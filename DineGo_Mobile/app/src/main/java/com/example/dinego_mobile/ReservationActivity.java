package com.example.dinego_mobile;

import android.app.DatePickerDialog;
import android.app.TimePickerDialog;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.util.Log;
import android.widget.Button;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.TimePicker;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Locale;

import Data.DatabaseHelper;

public class ReservationActivity extends AppCompatActivity {
    private TextView tvRestaurantName;
    private EditText editDate, editTime, editQuantity, editNote;
    private Button btnSubmit;
    private Calendar calendar;
    private DatabaseHelper databaseHelper;
    private int customerId = -1;
    private int restaurantId = -1; // Lưu ID nhà hàng

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_reservation);

        // Ánh xạ UI
        tvRestaurantName = findViewById(R.id.tv_selected_restaurant);
        editDate = findViewById(R.id.edit_date);
        editTime = findViewById(R.id.edit_time);
        editQuantity = findViewById(R.id.edit_quantity);
        editNote = findViewById(R.id.edit_note);
        btnSubmit = findViewById(R.id.btn_submit);

        // Nhận dữ liệu từ Intent
        Intent intent = getIntent();
        if (intent != null) {
            restaurantId = intent.getIntExtra("restaurant_id", -1);
            String restaurantName = intent.getStringExtra("restaurant_name");

            // Log kiểm tra dữ liệu từ Intent
            Log.d("DEBUG", "Nhận từ Intent - restaurant_id: " + restaurantId + ", restaurant_name: " + restaurantName);

            // Gán dữ liệu vào TextView thay vì Spinner
            if (restaurantName != null) {
                tvRestaurantName.setText(restaurantName);
            }
        }

        // Lấy customerId từ SharedPreferences
        SharedPreferences sharedPreferences = getSharedPreferences("UserSession", Context.MODE_PRIVATE);
        customerId = sharedPreferences.getInt("CUS_ID", -1);

        if (customerId == -1) {
            showLoginRequired();
            return;
        }

        calendar = Calendar.getInstance();
        databaseHelper = new DatabaseHelper();

        // Setup Date và Time picker
        editDate.setOnClickListener(v -> showDatePicker());
        editTime.setOnClickListener(v -> showTimePicker());

        // Submit đặt bàn
        btnSubmit.setOnClickListener(v -> submitReservation());
    }

    private void showLoginRequired() {
        Toast.makeText(this, "Vui lòng đăng nhập để đặt bàn", Toast.LENGTH_LONG).show();
        btnSubmit.setEnabled(false);
    }

    private void showDatePicker() {
        DatePickerDialog datePickerDialog = new DatePickerDialog(
                this,
                (view, year, month, dayOfMonth) -> {
                    calendar.set(Calendar.YEAR, year);
                    calendar.set(Calendar.MONTH, month);
                    calendar.set(Calendar.DAY_OF_MONTH, dayOfMonth);
                    updateDateEditText();
                },
                calendar.get(Calendar.YEAR),
                calendar.get(Calendar.MONTH),
                calendar.get(Calendar.DAY_OF_MONTH)
        );
        datePickerDialog.getDatePicker().setMinDate(System.currentTimeMillis() - 1000);
        datePickerDialog.show();
    }

    private void showTimePicker() {
        TimePickerDialog timePickerDialog = new TimePickerDialog(
                this,
                (view, hourOfDay, minute) -> {
                    calendar.set(Calendar.HOUR_OF_DAY, hourOfDay);
                    calendar.set(Calendar.MINUTE, minute);
                    updateTimeEditText();
                },
                calendar.get(Calendar.HOUR_OF_DAY),
                calendar.get(Calendar.MINUTE),
                true
        );
        timePickerDialog.show();
    }

    private void updateDateEditText() {
        SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy", Locale.getDefault());
        editDate.setText(sdf.format(calendar.getTime()));
    }

    private void updateTimeEditText() {
        SimpleDateFormat sdf = new SimpleDateFormat("HH:mm", Locale.getDefault());
        editTime.setText(sdf.format(calendar.getTime()));
    }

    private void submitReservation() {
        if (customerId == -1) {
            showLoginRequired();
            return;
        }

        if (restaurantId == -1) {
            Toast.makeText(this, "Lỗi: Không có thông tin nhà hàng", Toast.LENGTH_SHORT).show();
            return;
        }

        String dateStr = editDate.getText().toString().trim();
        String timeStr = editTime.getText().toString().trim();
        String quantityStr = editQuantity.getText().toString().trim();
        String noteStr = editNote.getText().toString().trim();

        if (dateStr.isEmpty() || timeStr.isEmpty() || quantityStr.isEmpty()) {
            Toast.makeText(this, "Vui lòng điền đầy đủ thông tin", Toast.LENGTH_SHORT).show();
            return;
        }

        // Chuyển đổi ngày giờ về đúng định dạng SQL Server
        SimpleDateFormat inputFormat = new SimpleDateFormat("dd/MM/yyyy HH:mm", Locale.getDefault());
        SimpleDateFormat sqlFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss", Locale.getDefault());

        try {
            java.util.Date date = inputFormat.parse(dateStr + " " + timeStr);
            String formattedDate = sqlFormat.format(date);

            Log.d("DEBUG", "Formatted DateTime for SQL: " + formattedDate);

            databaseHelper.createReservation(customerId, restaurantId, formattedDate, quantityStr, noteStr,
                    (success, reservationId) -> {
                        if (success) {
                            Toast.makeText(this, "Đặt bàn thành công! ID: " + reservationId, Toast.LENGTH_LONG).show();
                            finish(); // Đóng màn hình sau khi đặt bàn thành công
                        } else {
                            Toast.makeText(this, "Đặt bàn thất bại, vui lòng thử lại", Toast.LENGTH_SHORT).show();
                        }
                    });

        } catch (Exception e) {
            e.printStackTrace();
            Toast.makeText(this, "Lỗi định dạng ngày/giờ", Toast.LENGTH_SHORT).show();
        }
    }

}
