package com.example.dinego_mobile;

import android.content.Intent;
import android.os.Bundle;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class LoginActivity extends AppCompatActivity {
    EditText edtUsername, edtPassword;
    Button btnLogin;
    private TextView tvForgotPassword;
    private ExecutorService executorService;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        edtUsername = findViewById(R.id.edtUsername);
        edtPassword = findViewById(R.id.edtPassword);
        btnLogin = findViewById(R.id.btnLogin);
        tvForgotPassword = findViewById(R.id.tvForgotPassword);
        executorService = Executors.newSingleThreadExecutor();

        // Xử lý sự kiện khi bấm "Forgot your password?"
        tvForgotPassword.setOnClickListener(v -> {
            Intent intent = new Intent(LoginActivity.this, LoginActivity.class);
            intent.putExtra("SHOW_FORGOT_PASSWORD", true);
            startActivity(intent);
        });

        btnLogin.setOnClickListener(v -> {
            String username = edtUsername.getText().toString().trim();
            String password = edtPassword.getText().toString().trim();

            if (username.isEmpty() || password.isEmpty()) {
                Toast.makeText(LoginActivity.this, "Vui lòng nhập đầy đủ thông tin!", Toast.LENGTH_SHORT).show();
                return;
            }

            // Thực thi đăng nhập trên luồng khác
            executorService.execute(() -> {
                boolean isSuccess = checkLogin(username, password);
                runOnUiThread(() -> {
                    if (isSuccess) {
                        Toast.makeText(LoginActivity.this, "Đăng nhập thành công!", Toast.LENGTH_SHORT).show();
                        Intent intent = new Intent(LoginActivity.this, HomeActivity.class);
                        startActivity(intent);
                        finish();
                    } else {
                        Toast.makeText(LoginActivity.this, "Sai tài khoản hoặc mật khẩu!", Toast.LENGTH_SHORT).show();
                    }
                });
            });
        });

        // Kiểm tra nếu mở từ "Forgot Password"
        boolean showForgotPassword = getIntent().getBooleanExtra("SHOW_FORGOT_PASSWORD", false);
        if (showForgotPassword) {
            setContentView(R.layout.activity_forgetpassword);
            handleForgotPassword();
        }
    }

    private boolean checkLogin(String username, String password) {
        boolean isSuccess = false;
        Connection connection = Data.DatabaseHelper.getConnection();
        if (connection != null) {
            try {
                String sql = "SELECT * FROM customers WHERE cus_username = ? AND cus_password = ?";
                PreparedStatement stmt = connection.prepareStatement(sql);
                stmt.setString(1, username);
                stmt.setString(2, password);
                ResultSet rs = stmt.executeQuery();
                if (rs.next()) {
                    isSuccess = true;
                }
                rs.close();
                stmt.close();
                connection.close();
            } catch (SQLException e) {
                e.printStackTrace();
            }
        }
        return isSuccess;
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        executorService.shutdown();
    }

    private void handleForgotPassword() {
        EditText edtEmail = findViewById(R.id.edtEmail);
        Button btnResetPassword = findViewById(R.id.btnResetPassword);
        TextView tvBackToLogin = findViewById(R.id.tvBackToLogin);

        btnResetPassword.setOnClickListener(v -> {
            String email = edtEmail.getText().toString().trim();
            if (!email.isEmpty()) {
                Toast.makeText(LoginActivity.this, "Link đặt lại mật khẩu đã gửi đến " + email, Toast.LENGTH_SHORT).show();
                finish(); // Quay lại màn hình login
            } else {
                Toast.makeText(LoginActivity.this, "Vui lòng nhập email!", Toast.LENGTH_SHORT).show();
            }
        });

        tvBackToLogin.setOnClickListener(v -> finish());
    }
}