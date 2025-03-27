package Fragment;

import android.app.AlertDialog;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;

import com.example.dinego_mobile.LoginActivity;
import com.example.dinego_mobile.R;
import Data.DatabaseHelper;

public class ProfileFragment extends Fragment {
    private TextView userName, userEmail, userPhone, userAddress;
    private Button editProfileButton;
    private int customerId = -1; // ID khách hàng, có thể lấy từ SharedPreferences hoặc Argument
    private DatabaseHelper databaseHelper; // Đối tượng truy vấn DB

    public ProfileFragment() {
        // Required empty public constructor
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_profile, container, false);

        databaseHelper = new DatabaseHelper();

        // Ánh xạ UI
        userName = view.findViewById(R.id.user_name);
        userEmail = view.findViewById(R.id.user_email);
        userPhone = view.findViewById(R.id.user_phone);
        userAddress = view.findViewById(R.id.user_address);
        editProfileButton = view.findViewById(R.id.edit_profile_button);
        SharedPreferences sharedPreferences = getActivity().getSharedPreferences("UserSession", getContext().MODE_PRIVATE);
        customerId = sharedPreferences.getInt("CUS_ID", -1); // Mặc định -1 nếu không tìm thấy

        // Lấy dữ liệu khách hàng từ Database
        databaseHelper.getCustomerById(customerId, customer -> {
            if (customer != null) {
                userName.setText(customer.getName());
                userEmail.setText(customer.getEmail());
                userPhone.setText(customer.getPhone());
                userAddress.setText(customer.getAddress());
            } else {
                Log.e("ProfileFragment", "Không tìm thấy khách hàng.");
            }
        });
        Button btnLogout = view.findViewById(R.id.logout_button);

        btnLogout.setOnClickListener(v -> {
            // Xóa thông tin đăng nhập trong SharedPreferences
            SharedPreferences.Editor editor = sharedPreferences.edit();
            editor.clear();
            editor.apply();

            // Chuyển về màn hình đăng nhập
            Intent intent = new Intent(getActivity(), LoginActivity.class);
            intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TASK); // Xóa hết stack
            startActivity(intent);
        });
        // Xử lý khi nhấn nút Edit Profile
        editProfileButton.setOnClickListener(v -> openEditProfileDialog());

        return view;
    }

    private void openEditProfileDialog() {
        // Tạo Dialog
        AlertDialog.Builder builder = new AlertDialog.Builder(getContext());
        LayoutInflater inflater = getLayoutInflater();
        View dialogView = inflater.inflate(R.layout.dialog_edit_profile, null);
        builder.setView(dialogView);

        // Lấy reference đến các EditText trong dialog
        EditText editName = dialogView.findViewById(R.id.edit_name);
        EditText editEmail = dialogView.findViewById(R.id.edit_email);
        EditText editPhone = dialogView.findViewById(R.id.edit_phone);
        EditText editAddress = dialogView.findViewById(R.id.edit_address);
        Button saveButton = dialogView.findViewById(R.id.save_button);

        // Hiển thị thông tin cũ
        editName.setText(userName.getText().toString());
        editEmail.setText(userEmail.getText().toString());
        editPhone.setText(userPhone.getText().toString());
        editAddress.setText(userAddress.getText().toString());

        AlertDialog dialog = builder.create();
        dialog.show();

        // Xử lý khi nhấn "Save"
        saveButton.setOnClickListener(v -> {
            String newName = editName.getText().toString().trim();
            String newEmail = editEmail.getText().toString().trim();
            String newPhone = editPhone.getText().toString().trim();
            String newAddress = editAddress.getText().toString().trim();

            // Cập nhật Database
            databaseHelper.updateCustomerInfo(customerId, newName, newEmail, newPhone, newAddress, success -> {
                if (success) {
                    // Cập nhật giao diện
                    userName.setText(newName);
                    userEmail.setText(newEmail);
                    userPhone.setText(newPhone);
                    userAddress.setText(newAddress);

                    Toast.makeText(getContext(), "Cập nhật thành công!", Toast.LENGTH_SHORT).show();
                    dialog.dismiss(); // Đóng dialog
                } else {
                    Toast.makeText(getContext(), "Cập nhật thất bại. Vui lòng thử lại!", Toast.LENGTH_SHORT).show();
                }
            });
        });

    }
}
