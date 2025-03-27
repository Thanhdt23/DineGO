package com.example.dinego_mobile;

import android.os.Bundle;
import android.util.Log;
import android.widget.TextView;
import androidx.appcompat.app.AppCompatActivity;
import Data.DatabaseHelper;
import Models.Customer;

public class ProfileActivity extends AppCompatActivity {
    private TextView userName, userEmail, userPhone, userAddress;
    private int customerId = 2; // ID khách hàng, có thể lấy từ Intent hoặc SharedPreferences

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.fragment_profile);

        // Ánh xạ UI
        userName = findViewById(R.id.user_name);
        userEmail = findViewById(R.id.user_email);
        userPhone = findViewById(R.id.user_phone);
        userAddress = findViewById(R.id.user_address);

        // Lấy dữ liệu khách hàng từ database
        new DatabaseHelper().getCustomerById(customerId, customer -> {
            if (customer != null) {
                userName.setText(customer.getName());
                userEmail.setText(customer.getEmail());
                userPhone.setText(customer.getPhone());
                userAddress.setText("Updating..."); // Nếu có địa chỉ, cần cập nhật trong database
            } else {
                Log.e("ProfileActivity", "Không tìm thấy khách hàng.");
            }
        });
    }
}