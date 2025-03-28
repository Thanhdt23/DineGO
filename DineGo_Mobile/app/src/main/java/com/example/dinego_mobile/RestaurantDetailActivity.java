package com.example.dinego_mobile;

import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ScrollView;
import android.widget.TextView;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.bumptech.glide.Glide;
import com.bumptech.glide.load.engine.DiskCacheStrategy;

public class RestaurantDetailActivity extends AppCompatActivity {
    private ImageView restaurantImage;
    private TextView restaurantName, restaurantAddress, restaurantType,restaurantInformation;
    private Button btnBack, btnMakeReservation;
    private ScrollView scrollView;
    private LinearLayout bottomButtons;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_restaurant_detail);

        // Ánh xạ UI
        restaurantImage = findViewById(R.id.restaurant_detail_image);
        restaurantName = findViewById(R.id.restaurant_detail_name);
        restaurantAddress = findViewById(R.id.restaurant_detail_address);
        restaurantType = findViewById(R.id.restaurant_detail_type);
        restaurantInformation = findViewById(R.id.restaurant_detail_information);
        btnBack = findViewById(R.id.btn_back);
        btnMakeReservation = findViewById(R.id.btn_make_reservation);
        scrollView = findViewById(R.id.restaurant_scroll_view);
        bottomButtons = findViewById(R.id.bottom_buttons);

        // Nhận dữ liệu từ Intent
        Intent intent = getIntent();
        int id = intent.getIntExtra("restaurant_id", -1);
        String name = intent.getStringExtra("restaurant_name");
        String address = intent.getStringExtra("restaurant_address");
        String type = intent.getStringExtra("restaurant_type");
        String imageUrl = intent.getStringExtra("restaurant_image");
        String resInformation = intent.getStringExtra("restaurant_information");


        // Set dữ liệu
        restaurantName.setText(name);
        restaurantAddress.setText(address);
        restaurantType.setText(type);
        restaurantInformation.setText(resInformation);

        // Load ảnh với Glide
        Glide.with(this)
                .load(imageUrl)
                .diskCacheStrategy(DiskCacheStrategy.ALL)
                .error(R.drawable.dinego_logo)
                .into(restaurantImage);


        TextView addressTextView = findViewById(R.id.restaurant_detail_address);
        addressTextView.setOnClickListener(view -> {
            String addressUri = addressTextView.getText().toString();
            Uri gmmIntentUri = Uri.parse("geo:0,0?q=" + Uri.encode(address));
            Intent mapIntent = new Intent(Intent.ACTION_VIEW, gmmIntentUri);

            // Thử mở bằng Google Maps trước
            mapIntent.setPackage("com.google.android.apps.maps");

            if (mapIntent.resolveActivity(getPackageManager()) != null) {
                startActivity(mapIntent);
            } else {
                // Nếu không có Google Maps, hiển thị danh sách ứng dụng thay thế
                Intent chooser = Intent.createChooser(mapIntent, "Chọn ứng dụng để mở bản đồ");
                startActivity(chooser);
            }
        });

        scrollView.getViewTreeObserver().addOnScrollChangedListener(() -> {
            View view = (View) scrollView.getChildAt(scrollView.getChildCount() - 1);
            int diff = view.getBottom() - (scrollView.getHeight() + scrollView.getScrollY());

            if (diff <= 0) {
                bottomButtons.setVisibility(View.VISIBLE);
            } else {
                bottomButtons.setVisibility(View.GONE);
            }
        });

        // Xử lý nút Back
        btnBack.setOnClickListener(v -> finish());

        // Xử lý nút Make Reservation
        btnMakeReservation.setOnClickListener(v -> {
            Intent reservationIntent = new Intent(RestaurantDetailActivity.this, ReservationActivity.class);
            reservationIntent.putExtra("restaurant_id", id);
            reservationIntent.putExtra("restaurant_name", name);
            reservationIntent.putExtra("restaurant_address", address);
//            SharedPreferences sharedPreferences = getSharedPreferences("UserSession", Context.MODE_PRIVATE);
//            SharedPreferences.Editor editor = sharedPreferences.edit();
//            editor.putInt("RES_ID", id);
//            editor.apply();
            startActivity(reservationIntent);
        });


    }
}
