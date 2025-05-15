using AutoMapper;
using Vellora.ECommerce.API.Models;
using Vellora.ECommerce.API.DTOs.Request;
using Vellora.ECommerce.API.DTOs.Response;
using Vellora.ECommerce.API.Repositories.IRepositories;
using Vellora.ECommerce.API.Services.IServices;

namespace Vellora.ECommerce.API.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CartService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // ✅ Get Cart by User ID
        public async Task<CartResponse> GetCartByUserIdAsync(string userId)
        {
            var cart = await _unitOfWork.Cart.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                return new CartResponse { UserId = userId, CartItems = new List<CartItemResponse>() };
            }

            return _mapper.Map<CartResponse>(cart);
        }

        // ✅ Add Item to Cart
        public async Task<CartResponse> AddItemToCartAsync(string userId, CartItemRequest request)
        {
            // ✅ Ensure ProductId is provided
            if (request.ProductId == null)
                throw new ArgumentException("ProductId is required.");

            // ✅ Get or create the user's cart
            var cart = await _unitOfWork.Cart.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId, CartItems = new List<CartItem>() };
                await _unitOfWork.Cart.AddAsync(cart);
                await _unitOfWork.SaveAsync(); // Ensure CartId is generated before adding items
            }

            // ✅ Check if product exists
            var product = await _unitOfWork.Product.GetByIdAsync(request.ProductId.Value); // Use .Value to get non-null int
            if (product == null)
                throw new KeyNotFoundException("Product not found.");

            // ✅ Check if item already exists in cart
            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == request.ProductId.Value);
            if (cartItem == null)
            {
                // ✅ Manually create CartItem instead of mapping (to ensure CartId is set)
                cartItem = new CartItem
                {
                    ProductId = request.ProductId.Value, // Ensure non-null ProductId
                    Quantity = request.Quantity,
                    CartId = cart.Id // ✅ Ensure correct cart association
                };
                cart.CartItems.Add(cartItem);
            }
            else
            {
                // ✅ Update quantity if item already exists
                cartItem.Quantity += request.Quantity;
            }

            await _unitOfWork.SaveAsync();
            return _mapper.Map<CartResponse>(cart);
        }


        public async Task<CartResponse> UpdateCartItemAsync(string userId, int cartItemId, CartItemUpdateRequest request)
        {
            var cart = await _unitOfWork.Cart.GetCartByUserIdAsync(userId);
            if (cart == null) throw new KeyNotFoundException("Cart not found.");

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
            if (cartItem == null) throw new KeyNotFoundException("Cart item not found.");

            // ✅ Only update Quantity, ignore ProductId
            cartItem.Quantity = request.Quantity;

            await _unitOfWork.SaveAsync();
            return _mapper.Map<CartResponse>(cart);
        }


        // ✅ Remove Cart Item
        public async Task<bool> RemoveCartItemAsync(string userId, int cartItemId)
        {
            var cart = await _unitOfWork.Cart.GetCartByUserIdAsync(userId);
            if (cart == null) return false;

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
            if (cartItem == null) return false;

            cart.CartItems.Remove(cartItem);
            await _unitOfWork.SaveAsync();
            return true;
        }

        // ✅ Clear Entire Cart
        public async Task<bool> ClearCartAsync(string userId)
        {
            var cart = await _unitOfWork.Cart.GetCartByUserIdAsync(userId);
            if (cart == null) return false;

            cart.CartItems.Clear();
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
