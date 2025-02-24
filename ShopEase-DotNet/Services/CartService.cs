using AutoMapper;
using ShopEase.Models;
using ShopEase.DTOs.Request;
using ShopEase.DTOs.Response;
using ShopEase.Repositories.IRepositories;
using ShopEase.Services.IServices;

namespace ShopEase.Services
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

        public async Task<CartResponse> GetCartByUserIdAsync(string userId)
        {
            var cart = await _unitOfWork.Cart.GetCartByUserIdAsync(userId);
            return _mapper.Map<CartResponse>(cart);
        }

        public async Task<CartResponse> AddItemToCartAsync(string userId, CartItemRequest request)
        {
            var cart = await _unitOfWork.Cart.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId, CartItems = new List<CartItem>() };
                await _unitOfWork.Cart.AddAsync(cart);
            }

            var product = await _unitOfWork.Product.GetByIdAsync(request.ProductId);
            if (product == null) throw new KeyNotFoundException("Product not found.");

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == request.ProductId);
            if (cartItem == null)
            {
                cartItem = new CartItem { ProductId = request.ProductId, Quantity = request.Quantity, CartId = cart.Id };
                cart.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += request.Quantity;
            }

            await _unitOfWork.SaveAsync();
            return _mapper.Map<CartResponse>(cart);
        }

        public async Task<CartResponse> UpdateCartItemAsync(string userId, int cartItemId, int quantity)
        {
            var cart = await _unitOfWork.Cart.GetCartByUserIdAsync(userId);
            if (cart == null) throw new KeyNotFoundException("Cart not found.");

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
            if (cartItem == null) throw new KeyNotFoundException("Cart item not found.");

            cartItem.Quantity = quantity;

            await _unitOfWork.SaveAsync();
            return _mapper.Map<CartResponse>(cart);
        }

        public async Task<bool> RemoveCartItemAsync(string userId, int cartItemId)
        {
            var cart = await _unitOfWork.Cart.GetCartByUserIdAsync(userId);
            if (cart == null) throw new KeyNotFoundException("Cart not found.");

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == cartItemId);
            if (cartItem == null) return false;

            cart.CartItems.Remove(cartItem);
            await _unitOfWork.SaveAsync();
            return true;
        }

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
