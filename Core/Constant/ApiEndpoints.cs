/// <summary>
/// Represents the constants required for API configuration.
/// </summary>
namespace Core.Constant
{
    /// <summary>
    /// Provides API endpoint constants for various resources.
    /// </summary>
    public static class ApiEndpoints
    {
        /// <summary>
        /// Endpoint for product-related operations.
        /// </summary>
        public const string PRODUCT = "Product";

        /// <summary>
        /// Endpoint for category-related operations.
        /// </summary>
        public const string CATEGORY = "Category";

        /// <summary>
        /// Endpoint for retrieving a product by its ID.
        /// /// </summary>
        public const string PRODUCT_BY_ID = "Product/id?id=";

        public const string NOTIFICATION = "Notification";
        public const string NOTIFICATION_BY_ID = "Notification/id?id=";

        public const string CUSTOMER = "Customer";
        public const string CUSTOMER_BY_ID = "Customer/id?id=";
        public const string CUSTOMER_FORGET_PASSWORD = "Customer/forgetpassword?email=";
        public const string CUSTOMER_CHECK_USERNAME = "Customer/username?username=";
        public static string GetCustomerCheckLoginUrl(string username, string password)
        {
            return $"Customer/checklogin?username={username}&password={password}";
        }

        public const string BLOG = "Blog";
        public const string BLOG_BY_ID = "Blog/id?ID=";

        public const string ORDER = "Order";
        public const string ORDER_BY_ID = "Order/id?id=";
        public const string ORDER_BY_CUSTOMER = "Order/GetCusId?customerID=";

        public const string PAYMENT_BY_ID = "Payment/GetCusIdPayment?CustomerID=";

        public const string CART = "Cart";
        public const string CART_BY_CUSID = "Cart/CustomerID?CustomerID=";
        public const string CART_BY_ID = "Order/id?id=";

        public const string RESTAURANT = "Restaurant";

        public const string RESTAURANT_SEARCH = "Restaurant/search?name={0}&address={1}";

        public const string RESTAURANT_OWNER = "RestaurantOwner";
        public const string RESTAURANT_OWNER_BY_ID = "RestaurantOwner/id?id=";
        public const string RESERVATION ="reservation";
        public const string RESTAURANT_OWNER_BY_CUS_ID ="RestaurantOwner/cusId?Id={0}";
        public const string RESERVATION_BY_CUSID ="Reservation/cus_id?cus_id=";
        public const string PAYMENT ="Payment";
        public const string PAYMENT_BY_CUSID ="Payment/cus_id?cus_id=";
    }
}