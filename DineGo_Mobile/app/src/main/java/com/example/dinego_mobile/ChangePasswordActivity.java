package com.example.dinego_mobile;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;
import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class ChangePasswordActivity extends AppCompatActivity {
    private EditText oldPassword, newPassword, confirmPassword;
    private Button changePasswordButton;
    private SharedPreferences sharedPreferences;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.change_password);

        oldPassword = findViewById(R.id.old_password);
        newPassword = findViewById(R.id.new_password);
        confirmPassword = findViewById(R.id.confirm_password);
        changePasswordButton = findViewById(R.id.change_password_button);
        Button backButton = findViewById(R.id.back_button);


        sharedPreferences = getSharedPreferences("UserPrefs", MODE_PRIVATE);
        String storedPassword = sharedPreferences.getString("cus_password", "");
        backButton.setOnClickListener(v -> finish()); // Quay lại màn hình trước
        changePasswordButton.setOnClickListener(v -> {
            String oldPassInput = oldPassword.getText().toString().trim();
            String newPassInput = newPassword.getText().toString().trim();
            String confirmPassInput = confirmPassword.getText().toString().trim();

            if (oldPassInput.isEmpty() || newPassInput.isEmpty() || confirmPassInput.isEmpty()) {
                Toast.makeText(ChangePasswordActivity.this, "Please input null information!", Toast.LENGTH_SHORT).show();
                return;
            }

            if (!newPassInput.equals(confirmPassInput)) {
                Toast.makeText(ChangePasswordActivity.this, "New password is not same!", Toast.LENGTH_SHORT).show();
                return;
            }

            SharedPreferences sharedPreferences = getSharedPreferences("UserSession", MODE_PRIVATE);
            String username = sharedPreferences.getString("USERNAME", "");

            // Kiểm tra mật khẩu cũ và cập nhật mật khẩu mới trên SQL Server
            ExecutorService executor = Executors.newSingleThreadExecutor();
            executor.execute(() -> {
                boolean isPasswordCorrect = checkOldPassword(username, oldPassInput);
                if (!isPasswordCorrect) {
                    runOnUiThread(() -> Toast.makeText(ChangePasswordActivity.this, "Old password is incorrect!", Toast.LENGTH_SHORT).show());
                    return;
                }

                boolean isUpdated = updatePassword(username, newPassInput);
                runOnUiThread(() -> {
                    if (isUpdated) {
                        SharedPreferences.Editor editor = sharedPreferences.edit();
                        editor.putString("PASSWORD", newPassInput);
                        editor.apply();

                        Toast.makeText(ChangePasswordActivity.this, "Change password succesfully!", Toast.LENGTH_SHORT).show();
                        finish();
                    } else {
                        Toast.makeText(ChangePasswordActivity.this, "Change password failed!", Toast.LENGTH_SHORT).show();
                    }
                });
            });
        });

    }
    private boolean checkOldPassword(String username, String oldPassword) {
        Connection connection = Data.DatabaseHelper.getConnection();
        if (connection != null) {
            try {
                String sql = "SELECT cus_password FROM customers WHERE cus_username = ?";
                PreparedStatement stmt = connection.prepareStatement(sql);
                stmt.setString(1, username);
                ResultSet rs = stmt.executeQuery();
                if (rs.next()) {
                    String storedPassword = rs.getString("cus_password");
                    return storedPassword.equals(oldPassword); // So sánh với mật khẩu nhập vào
                }
                rs.close();
                stmt.close();
            } catch (SQLException e) {
                e.printStackTrace();
            } finally {
                try {
                    connection.close();
                } catch (SQLException e) {
                    e.printStackTrace();
                }
            }
        }
        return false;
    }

    private boolean updatePassword(String username, String newPassword) {
        Connection connection = Data.DatabaseHelper.getConnection();
        if (connection != null) {
            try {
                String sql = "UPDATE customers SET cus_password = ? WHERE cus_username = ?";
                PreparedStatement stmt = connection.prepareStatement(sql);
                stmt.setString(1, newPassword);
                stmt.setString(2, username);
                int rowsAffected = stmt.executeUpdate();
                stmt.close();
                return rowsAffected > 0;
            } catch (SQLException e) {
                e.printStackTrace();
            } finally {
                try {
                    connection.close();
                } catch (SQLException e) {
                    e.printStackTrace();
                }
            }
        }
        return false;
    }

}
