package com.example.dinego_mobile;

import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

public class LoginActivity extends AppCompatActivity {
    EditText edtUsername, edtPassword;
    Button btnLogin;
    private TextView tvForgotPassword;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        edtUsername = findViewById(R.id.edtUsername);
        edtPassword = findViewById(R.id.edtPassword);
        btnLogin = findViewById(R.id.btnLogin);
        tvForgotPassword = findViewById(R.id.tvForgotPassword);

        // Xử lý sự kiện khi bấm "Forgot your password?"
        tvForgotPassword.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(LoginActivity.this, LoginActivity.class);
                intent.putExtra("SHOW_FORGOT_PASSWORD", true);
                startActivity(intent);
            }
        });

        btnLogin.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String username = edtUsername.getText().toString();
                String password = edtPassword.getText().toString();
                new LoginTask().execute(username, password);
                Intent intent = new Intent(LoginActivity.this, HomeActivity.class);
                startActivity(intent);
            }
        });

        // Kiểm tra nếu mở từ "Forgot Password"
        boolean showForgotPassword = getIntent().getBooleanExtra("SHOW_FORGOT_PASSWORD", false);
        if (showForgotPassword) {
            setContentView(R.layout.activity_forgetpassword);
            handleForgotPassword();
        }
    }

    @SuppressWarnings("deprecation")
    class LoginTask extends AsyncTask<String, Void, Boolean> {
        @Override
        protected Boolean doInBackground(String... params) {
            boolean isSuccess = false;
            Connection connection = Data.DatabaseHelper.getConnection();
            if (connection != null) {
                try {
                    String sql = "SELECT * FROM customers WHERE cus_username = ? AND cus_password = ?";
                    PreparedStatement stmt = connection.prepareStatement(sql);
                    stmt.setString(1, params[0]);
                    stmt.setString(2, params[1]);
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
        protected void onPostExecute(Boolean result) {
            if (result) {
                Toast.makeText(LoginActivity.this, "Đăng nhập thành công!", Toast.LENGTH_SHORT).show();
            } else {
                Toast.makeText(LoginActivity.this, "Sai tài khoản hoặc mật khẩu!", Toast.LENGTH_SHORT).show();
            }
        }
    }

    private void handleForgotPassword() {
        EditText edtEmail = findViewById(R.id.edtEmail);
        Button btnResetPassword = findViewById(R.id.btnResetPassword);
        TextView tvBackToLogin = findViewById(R.id.tvBackToLogin);

        btnResetPassword.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String email = edtEmail.getText().toString();
                if (!email.isEmpty()) {
                    Toast.makeText(LoginActivity.this, "Link đặt lại mật khẩu đã gửi đến " + email, Toast.LENGTH_SHORT).show();
                    finish(); // Quay lại màn hình login
                } else {
                    Toast.makeText(LoginActivity.this, "Vui lòng nhập email!", Toast.LENGTH_SHORT).show();
                }
            }
        });

        tvBackToLogin.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                finish(); // Đóng màn hình quên mật khẩu để quay về Login
            }
        });
    }
}