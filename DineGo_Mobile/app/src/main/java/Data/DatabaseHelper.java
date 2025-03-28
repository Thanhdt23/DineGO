package Data;

import android.content.Context;
import android.os.Handler;
import android.os.Looper;
import android.util.Log;
import android.widget.Toast;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

import Models.Customer;
import Models.Notification;
import Models.Reservation;
import Models.Restaurant;

public class DatabaseHelper {
    private static final String IP = "192.168.1.54"; // Địa chỉ SQL Server
    private static final String PORT = "1433"; // Cổng mặc định
    private static final String DATABASE_NAME = "DineGo_DB_CodeFirst";
    private static final String USERNAME = "sa";
    private static final String PASSWORD = "123456";

    public static Connection getConnection() {
        Connection connection = null;
        String connectionUrl = "jdbc:jtds:sqlserver://" + IP + ":" + PORT + "/" + DATABASE_NAME + ";"
                + "user=" + USERNAME + ";"
                + "password=" + PASSWORD + ";"
                + "loginTimeout=30;";

        try {
            // Load jTDS Driver
            Class.forName("net.sourceforge.jtds.jdbc.Driver");

            // Kết nối
            connection = DriverManager.getConnection(connectionUrl);
            Log.d("DatabaseHelper", "✅ Kết nối thành công!");
        } catch (Exception e) {
            Log.e("DatabaseHelper", "❌ Lỗi kết nối SQL Server: " + e.getMessage(), e);
        }
        return connection;
    }

    public static void testConnection(Context context) {
        ExecutorService executorService = Executors.newSingleThreadExecutor();
        executorService.execute(() -> {
            try (Connection conn = getConnection()) {
                if (conn != null) {
                    Statement stmt = conn.createStatement();
                    ResultSet rs = stmt.executeQuery("SELECT TOP 1 * FROM customers");

                    if (rs.next()) {
                        Log.d("DatabaseHelper", "Dữ liệu: " + rs.getString(1));
                    }

                    new Handler(Looper.getMainLooper()).post(() ->
                            Toast.makeText(context, "Kết nối thành công!", Toast.LENGTH_SHORT).show()
                    );
                } else {
                    Log.e("DatabaseHelper", "Kết nối thất bại: Connection null");
                    new Handler(Looper.getMainLooper()).post(() ->
                            Toast.makeText(context, "Kết nối thất bại!", Toast.LENGTH_SHORT).show()
                    );
                }
            } catch (Exception e) {
                Log.e("DatabaseHelper", "Lỗi kết nối SQL Server: " + e.getMessage(), e);
                new Handler(Looper.getMainLooper()).post(() ->
                        Toast.makeText(context, "Lỗi kết nối: " + e.getMessage(), Toast.LENGTH_SHORT).show()
                );
            }
        });
    }
    public void getCustomerById(int customerId, Callback<Customer> callback) {
        ExecutorService executorService = Executors.newSingleThreadExecutor();
        executorService.execute(() -> {
            Customer customer = null;
            try (Connection conn = getConnection()) {
                if (conn != null) {
                    String query = "SELECT cus_id, cus_name, cus_email, cus_phone, cus_address FROM customers WHERE cus_id = ?";
                    PreparedStatement stmt = conn.prepareStatement(query);
                    stmt.setInt(1, customerId);
                    ResultSet rs = stmt.executeQuery();

                    if (rs.next()) {
                        int id = rs.getInt("cus_id");
                        String name = rs.getString("cus_name");
                        String email = rs.getString("cus_email");
                        String phone = rs.getString("cus_phone");
                        String address = rs.getString("cus_address");
                        customer = new Customer(id, name, email, phone, address);
                    }
                    rs.close();
                    stmt.close();
                }
            } catch (Exception e) {
                Log.e("DB_ERROR", "Không thể lấy thông tin khách hàng: " + e.getMessage(), e);
            }

            Customer finalCustomer = customer;
            new Handler(Looper.getMainLooper()).post(() -> callback.onResult(finalCustomer));
        });
    }


    // Lấy danh sách thông báo
    public void getNotifications(Callback<List<Notification>> callback) {
        ExecutorService executorService = Executors.newSingleThreadExecutor();
        executorService.execute(() -> {
            List<Notification> notificationList = new ArrayList<>();
            try (Connection conn = getConnection()) {
                if (conn != null) {
                    String query = "SELECT noti_id, cus_id, re_id, noti_title, noti_content, noti_type, noti_date, noti_status FROM notifications";
                    Statement stmt = conn.createStatement();
                    ResultSet rs = stmt.executeQuery(query);

                    while (rs.next()) {
                        int id = rs.getInt("noti_id");
                        int customerId = rs.getInt("cus_id");
                        int restaurantId = rs.getInt("re_id");
                        String title = rs.getString("noti_title");
                        String content = rs.getString("noti_content");
                        String type = rs.getString("noti_type");
                        String date = rs.getString("noti_date");
                        String status = rs.getString("noti_status");

                        notificationList.add(new Notification(id, title, content, type, date, status, customerId, false));
                    }
                    rs.close();
                    stmt.close();
                }
            } catch (Exception e) {
                Log.e("DB_ERROR", "Không thể lấy dữ liệu thông báo: " + e.getMessage(), e);
            }

            new Handler(Looper.getMainLooper()).post(() -> callback.onResult(notificationList));
        });
    }

    // Lấy danh sách nhà hàng
    public void getRestaurants(Callback<List<Restaurant>> callback) {
        ExecutorService executorService = Executors.newSingleThreadExecutor();
        executorService.execute(() -> {
            List<Restaurant> restaurantList = new ArrayList<>();
            try (Connection conn = getConnection()) {
                if (conn != null) {
                    String query = "SELECT res_id, res_name, res_address, res_phone, res_images, res_information, res_price FROM restaurants";
                    Statement stmt = conn.createStatement();
                    ResultSet rs = stmt.executeQuery(query);

                    while (rs.next()) {
                        int id=rs.getInt("res_id");
                        String name = rs.getString("res_name");
                        String address = rs.getString("res_address");
                        String phone = rs.getString("res_phone");
                        String image = rs.getString("res_images");
                        String information = rs.getString("res_information");
                        Double price = rs.getDouble("res_price");


                        restaurantList.add(new Restaurant(id, name, address, phone, image, information, price));
                    }
                    rs.close();
                    stmt.close();
                }
            } catch (Exception e) {
                Log.e("DB_ERROR", "Không thể lấy dữ liệu: " + e.getMessage(), e);
            }

            new Handler(Looper.getMainLooper()).post(() -> callback.onResult(restaurantList));
        });
    }

    public void getReservations(Callback<List<Reservation>> callback) {
        ExecutorService executorService = Executors.newSingleThreadExecutor();
        executorService.execute(() -> {
            List<Reservation> reservationList = new ArrayList<>();
            try (Connection conn = getConnection()) {
                if (conn != null) {
                    String query = "SELECT re_id, cus_id, res_id, re_date , re_quantity , re_status , re_note FROM reservations";
                    Statement stmt = conn.createStatement();
                    ResultSet rs = stmt.executeQuery(query);

                    while (rs.next()) {
                        int id = rs.getInt("re_id");
                        int customerId = rs.getInt("cus_id");
                        int resId = rs.getInt("res_id");
                        String reservationDate = rs.getString("re_date");
                        String reservationQuantity = rs.getString("re_quantity");
                        String reservationStatus = rs.getString("re_status");
                        String reservationNote = rs.getString("re_note");

                        reservationList.add(new Reservation(id, reservationStatus, reservationDate, reservationQuantity, reservationNote, customerId, resId));
                    }
                    rs.close();
                    stmt.close();
                }
            } catch (Exception e) {
                Log.e("DB_ERROR", "Không thể lấy dữ liệu thông báo: " + e.getMessage(), e);
            }

            new Handler(Looper.getMainLooper()).post(() -> callback.onResult(reservationList));
        });
    }

    public void searchRestaurantsByName(String query, Callback<List<Restaurant>> callback) {
        ExecutorService executorService = Executors.newSingleThreadExecutor();
        executorService.execute(() -> {
            List<Restaurant> filteredRestaurants = new ArrayList<>();
            try (Connection conn = getConnection()) {
                if (conn != null) {
                    String sql = "SELECT res_id, res_name, res_address, res_type, res_images FROM restaurants WHERE res_name LIKE ?";
                    PreparedStatement stmt = conn.prepareStatement(sql);
                    stmt.setString(1, "%" + query + "%"); // Tìm kiếm gần đúng

                    ResultSet rs = stmt.executeQuery();
                    while (rs.next()) {
                        int id = rs.getInt("res_id");
                        String name = rs.getString("res_name");
                        String address = rs.getString("res_address");
                        String type = rs.getString("res_type");
                        String image = rs.getString("res_images");

                        filteredRestaurants.add(new Restaurant( id, name, address, type, image));
                    }
                    rs.close();
                    stmt.close();
                }
            } catch (Exception e) {
                Log.e("DB_ERROR", "Lỗi khi tìm kiếm nhà hàng: " + e.getMessage(), e);
            }

            new Handler(Looper.getMainLooper()).post(() -> callback.onResult(filteredRestaurants));
        });
    }


    public void searchRestaurantsByType(String resType, Callback<List<Restaurant>> callback) {
        ExecutorService executorService = Executors.newSingleThreadExecutor();
        executorService.execute(() -> {
            List<Restaurant> filteredRestaurants = new ArrayList<>();
            try (Connection conn = getConnection()) {
                if (conn != null) {
                    String sql = "SELECT res_id, res_name, res_address, res_type, res_images FROM restaurants WHERE res_type LIKE ?";
                    PreparedStatement stmt = conn.prepareStatement(sql);
                    stmt.setString(1, "%" + resType + "%"); // Tìm kiếm theo loại

                    ResultSet rs = stmt.executeQuery();
                    while (rs.next()) {
                        int id = rs.getInt("res_id");

                        String name = rs.getString("res_name");
                        String address = rs.getString("res_address");
                        String type = rs.getString("res_type");
                        String image = rs.getString("res_images");

                        filteredRestaurants.add(new Restaurant(id,name, address, type, image));
                    }
                    rs.close();
                    stmt.close();
                }
            } catch (Exception e) {
                Log.e("DB_ERROR", "Lỗi khi tìm kiếm nhà hàng theo loại: " + e.getMessage(), e);
            }

            new Handler(Looper.getMainLooper()).post(() -> callback.onResult(filteredRestaurants));
        });
    }

    public void updateCustomerInfo(int customerId, String name, String email, String phone, String address, OnUpdateCallback callback) {
        ExecutorService executorService = Executors.newSingleThreadExecutor();
        executorService.execute(() -> {
            boolean success = false;
            try (Connection conn = getConnection()) {
                if (conn != null) {
                    String sql = "UPDATE customers SET cus_name = ?, cus_email = ?, cus_phone = ?, cus_address = ? WHERE cus_id = ?";
                    PreparedStatement stmt = conn.prepareStatement(sql);
                    stmt.setString(1, name);
                    stmt.setString(2, email);
                    stmt.setString(3, phone);
                    stmt.setString(4, address);
                    stmt.setInt(5, customerId);

                    int rowsAffected = stmt.executeUpdate();
                    success = rowsAffected > 0;

                    stmt.close();
                }
            } catch (Exception e) {
                Log.e("DB_ERROR", "Lỗi khi cập nhật thông tin khách hàng: " + e.getMessage(), e);
            }

            boolean finalSuccess = success;
            new Handler(Looper.getMainLooper()).post(() -> callback.onResult(finalSuccess));
        });
    }

    public void createReservation(int customerId, int restaurantId, String date,
                                  String quantity, String note, OnCreateReservationCallback callback) {
        ExecutorService executorService = Executors.newSingleThreadExecutor();
        executorService.execute(() -> {
            boolean success = false;
            int reservationId = -1;
            try (Connection conn = getConnection()) {
                if (conn != null) {
                    // Sử dụng CAST để đảm bảo chuyển đổi đúng kiểu DATETIME2
                    String sql = "INSERT INTO reservations (cus_id, res_id, re_date, re_quantity, re_status, re_note) " +
                            "VALUES (?, ?, CAST(? AS DATETIME2(7)), ?, 'Pending', ?)";
                    PreparedStatement stmt = conn.prepareStatement(sql, Statement.RETURN_GENERATED_KEYS);
                    stmt.setInt(1, customerId);
                    stmt.setInt(2, restaurantId);
                    stmt.setString(3, date); // Đã được định dạng chuẩn từ Fragment
                    stmt.setString(4, quantity);
                    stmt.setString(5, note);

                    int rowsAffected = stmt.executeUpdate();
                    if (rowsAffected > 0) {
                        ResultSet rs = stmt.getGeneratedKeys();
                        if (rs.next()) {
                            reservationId = rs.getInt(1);
                        }
                        success = true;
                    }
                    stmt.close();
                }
            } catch (Exception e) {
                Log.e("DB_ERROR", "Lỗi khi tạo reservation: " + e.getMessage(), e);
            }

            boolean finalSuccess = success;
            int finalReservationId = reservationId;
            new Handler(Looper.getMainLooper()).post(() ->
                    callback.onResult(finalSuccess, finalReservationId));
        });
    }

    public interface OnCreateReservationCallback {
        void onResult(boolean success, int reservationId);
    }


    // Interface callback để xử lý kết quả
    public interface OnUpdateCallback {
        void onResult(boolean success);
    }


    // Interface để xử lý callback
    public interface Callback<T> {
        void onResult(T result);
    }

}
