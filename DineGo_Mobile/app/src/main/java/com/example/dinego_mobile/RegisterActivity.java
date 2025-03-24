package com.example.dinego_mobile;

import android.app.DatePickerDialog;
import android.content.Intent;
import android.icu.text.SimpleDateFormat;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.util.Calendar;
import java.util.Date;
import java.util.Locale;

import Data.DatabaseHelper;

public class RegisterActivity extends AppCompatActivity {
    private EditText edtUsername, edtPassword, edtName, edtEmail, edtGender;

    private TextView edtBirthday;
    private Button btnRegister, btnBack;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_register);

        // Ánh xạ UI
        edtUsername = findViewById(R.id.edtUsername);
        edtPassword = findViewById(R.id.edtPassword);
        edtName = findViewById(R.id.edtName);
        edtEmail = findViewById(R.id.edtEmail);
        edtBirthday = findViewById(R.id.edtBirthday);
        edtGender = findViewById(R.id.edtGender);
        btnRegister = findViewById(R.id.btnRegister);
        btnBack = findViewById(R.id.btnBack);
        // Xử lý sự kiện khi nhấn nút Đăng ký
        btnRegister.setOnClickListener(view -> registerUser());
        btnBack.setOnClickListener(v -> finish()); // Quay lại trang trước đó
        edtBirthday.setOnClickListener(view -> showDatePicker());
    }
    private String convertDateFormat(String inputDate) {
        try {
            SimpleDateFormat inputFormat = new SimpleDateFormat("dd/MM/yyyy", Locale.getDefault());
            SimpleDateFormat outputFormat = new SimpleDateFormat("yyyy-MM-dd", Locale.getDefault());
            Date date = inputFormat.parse(inputDate);
            return outputFormat.format(date);
        } catch (Exception e) {
            Log.e("RegisterActivity", "Lỗi chuyển đổi ngày: " + e.getMessage());
            return null;
        }
    }
    private void showDatePicker() {
        Calendar calendar = Calendar.getInstance();
        int year = calendar.get(Calendar.YEAR);
        int month = calendar.get(Calendar.MONTH);
        int day = calendar.get(Calendar.DAY_OF_MONTH);

        DatePickerDialog datePickerDialog = new DatePickerDialog(this,
                (view, selectedYear, selectedMonth, selectedDay) -> {
                    // Định dạng ngày tháng: DD/MM/YYYY
                    String selectedDate = selectedDay + "/" + (selectedMonth + 1) + "/" + selectedYear;
                    edtBirthday.setText(selectedDate);
                },
                year, month, day);
        datePickerDialog.show();
    }
    private void registerUser() {
        String username = edtUsername.getText().toString().trim();
        String password = edtPassword.getText().toString().trim();
        String name = edtName.getText().toString().trim();
        String email = edtEmail.getText().toString().trim();
        String birthday = convertDateFormat(edtBirthday.getText().toString().trim());
        String gender = edtGender.getText().toString().trim();
        if (birthday == null) {
            runOnUiThread(() -> Toast.makeText(this, "Lỗi định dạng ngày tháng!", Toast.LENGTH_SHORT).show());
            return;
        }
        // Kiểm tra các trường bắt buộc
        if (username.isEmpty() || password.isEmpty() || name.isEmpty() || email.isEmpty() || birthday.isEmpty() || gender.isEmpty()) {
            Toast.makeText(this, "Vui lòng nhập đầy đủ thông tin!", Toast.LENGTH_SHORT).show();
            return;
        }

        new Thread(() -> {
            try (Connection conn = DatabaseHelper.getConnection()) {
                if (conn == null) {
                    Log.e("RegisterActivity", "Kết nối database thất bại!");
                    runOnUiThread(() -> Toast.makeText(this, "Lỗi kết nối database!", Toast.LENGTH_SHORT).show());
                    return;
                }

                // Lệnh SQL chèn dữ liệu (Chỉ còn 6 trường)
                String sql = "INSERT INTO customers (cus_username, cus_password, cus_name, cus_email, cus_birthday, cus_gender) VALUES (?, ?, ?, ?, ?, ?)";
                try (PreparedStatement stmt = conn.prepareStatement(sql)) {
                    stmt.setString(1, username);
                    stmt.setString(2, password);
                    stmt.setString(3, name);
                    stmt.setString(4, email);
                    stmt.setString(5, birthday);
                    stmt.setString(6, gender);

                    int rowsInserted = stmt.executeUpdate();
                    Log.d("RegisterActivity", "Số dòng chèn vào: " + rowsInserted);

                    runOnUiThread(() -> {
                        if (rowsInserted > 0) {
                            Toast.makeText(this, "Đăng ký thành công!", Toast.LENGTH_SHORT).show();
                            startActivity(new Intent(this, LoginActivity.class));
                            finish();
                        } else {
                            Toast.makeText(this, "Đăng ký thất bại!", Toast.LENGTH_SHORT).show();
                        }
                    });
                }
            } catch (Exception e) {
                Log.e("RegisterActivity", "Lỗi đăng ký: " + e.getMessage(), e);
                runOnUiThread(() -> Toast.makeText(this, "Lỗi: " + e.getMessage(), Toast.LENGTH_SHORT).show());
            }
        }).start();
    }


    }


