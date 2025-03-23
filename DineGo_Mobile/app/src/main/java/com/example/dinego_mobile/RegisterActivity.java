package com.example.dinego_mobile;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import java.sql.Connection;
import java.sql.PreparedStatement;

import Data.DatabaseHelper;

public class RegisterActivity extends AppCompatActivity {
    private EditText edtUsername, edtPassword, edtName, edtEmail, edtPhone;
    private Button btnRegister, btnBack; ;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_register);

        // Ánh xạ UI
        edtUsername = findViewById(R.id.edtUsername);
        edtPassword = findViewById(R.id.edtPassword);
        edtName = findViewById(R.id.edtName);
        edtEmail = findViewById(R.id.edtEmail);
        edtPhone = findViewById(R.id.edtPhone);
        btnRegister = findViewById(R.id.btnRegister);
        btnBack = findViewById(R.id.btnBack);
        // Xử lý sự kiện khi nhấn nút Đăng ký
        btnRegister.setOnClickListener(view -> registerUser());
        btnBack.setOnClickListener(v -> finish()); // Quay lại trang trước đó
    }

    private void registerUser() {
        // Lấy dữ liệu từ input
        String username = edtUsername.getText().toString().trim();
        String password = edtPassword.getText().toString().trim();
        String name = edtName.getText().toString().trim();
        String email = edtEmail.getText().toString().trim();
        String phone = edtPhone.getText().toString().trim();

        // Kiểm tra dữ liệu hợp lệ
        if (username.isEmpty() || password.isEmpty() || name.isEmpty() || email.isEmpty() || phone.isEmpty()) {
            Toast.makeText(this, "Vui lòng nhập đầy đủ thông tin!", Toast.LENGTH_SHORT).show();
            return;
        }

        // Thực hiện đăng ký trên luồng khác để tránh lag UI
        new Thread(() -> {
            try (Connection conn = DatabaseHelper.getConnection()) {
                if (conn != null) {
                    String sql = "INSERT INTO users (username, password, name, email, phone) VALUES (?, ?, ?, ?, ?)";
                    PreparedStatement stmt = conn.prepareStatement(sql);
                    stmt.setString(1, username);
                    stmt.setString(2, password); // Cần mã hóa mật khẩu
                    stmt.setString(3, name);
                    stmt.setString(4, email);
                    stmt.setString(5, phone);

                    int rowsInserted = stmt.executeUpdate();
                    if (rowsInserted > 0) {
                        runOnUiThread(() -> {
                            Toast.makeText(this, "Đăng ký thành công!", Toast.LENGTH_SHORT).show();
                            startActivity(new Intent(this, LoginActivity.class));
                            finish();
                        });
                    } else {
                        runOnUiThread(() -> Toast.makeText(this, "Đăng ký thất bại!", Toast.LENGTH_SHORT).show());
                    }
                } else {
                    runOnUiThread(() -> Toast.makeText(this, "Lỗi kết nối database!", Toast.LENGTH_SHORT).show());
                }
            } catch (Exception e) {
                Log.e("RegisterActivity", "Lỗi đăng ký: " + e.getMessage(), e);
                runOnUiThread(() -> Toast.makeText(this, "Lỗi: " + e.getMessage(), Toast.LENGTH_SHORT).show());
            }
        }).start();
    }

}
