package Data;

import android.content.Context;
import android.os.Handler;
import android.os.Looper;
import android.util.Log;
import android.widget.Toast;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

import Models.Restaurant;

public class DatabaseHelper {
    private static final String IP = "192.168.1.43"; // Địa chỉ SQL Server
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
                    String query = "SELECT res_name, res_address, res_phone, res_images FROM restaurants";
                    Statement stmt = conn.createStatement();
                    ResultSet rs = stmt.executeQuery(query);

                    while (rs.next()) {
                        String name = rs.getString("res_name");
                        String address = rs.getString("res_address");
                        String phone = rs.getString("res_phone");
                        String image = rs.getString("res_images");

                        restaurantList.add(new Restaurant(name, address, phone, image));
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

    // Interface để xử lý callback
    public interface Callback<T> {
        void onResult(T result);
    }

}
