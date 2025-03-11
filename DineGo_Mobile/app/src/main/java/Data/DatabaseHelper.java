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
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class DatabaseHelper {
    private static final String IP = "192.168.1.34"; // Địa chỉ SQL Server
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
}
