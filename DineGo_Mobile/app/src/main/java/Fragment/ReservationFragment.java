package Fragment;

import android.app.DatePickerDialog;
import android.app.TimePickerDialog;
import android.content.Context;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.DatePicker;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.TimePicker;
import android.widget.Toast;

import androidx.fragment.app.Fragment;

import com.example.dinego_mobile.R;

import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.List;
import java.util.Locale;

import Models.Restaurant;
import Data.DatabaseHelper;
import Models.Reservation;

public class ReservationFragment extends Fragment {
    private Spinner spinnerRestaurants;
    private EditText editDate, editTime, editQuantity, editNote;
    private Button btnSubmit;
    private Calendar calendar;
    private DatabaseHelper databaseHelper;
    private int customerId = -1;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_reservation, container, false);

        // Initialize views
        spinnerRestaurants = view.findViewById(R.id.spinner_restaurants);
        editDate = view.findViewById(R.id.edit_date);
        editTime = view.findViewById(R.id.edit_time);
        editQuantity = view.findViewById(R.id.edit_quantity);
        editNote = view.findViewById(R.id.edit_note);
        btnSubmit = view.findViewById(R.id.btn_submit);

        // Lấy customerId từ SharedPreferences
        try {
            SharedPreferences sharedPreferences = requireActivity().getSharedPreferences(
                    "UserSession", Context.MODE_PRIVATE);
            customerId = sharedPreferences.getInt("CUS_ID", -1);

            if (customerId == -1) {
                showLoginRequired();
                return view;
            }
        } catch (Exception e) {
            Log.e("ReservationFragment", "Error getting customerId", e);
            showLoginRequired();
            return view;
        }

        calendar = Calendar.getInstance();
        databaseHelper = new DatabaseHelper();

        // Load danh sách nhà hàng vào Spinner
        loadRestaurants();

        // Set up date picker
        editDate.setOnClickListener(v -> showDatePicker());

        // Set up time picker
        editTime.setOnClickListener(v -> showTimePicker());

        // Set up submit button
        btnSubmit.setOnClickListener(v -> submitReservation());

        return view;
    }

    private void showLoginRequired() {
        Toast.makeText(requireContext(),
                "Vui lòng đăng nhập để sử dụng tính năng đặt bàn",
                Toast.LENGTH_LONG).show();
        btnSubmit.setEnabled(false);
        btnSubmit.setAlpha(0.5f);
    }

    private void loadRestaurants() {
        databaseHelper.getRestaurants(new DatabaseHelper.Callback<List<Restaurant>>() {
            @Override
            public void onResult(List<Restaurant> result) {
                if (result != null && !result.isEmpty()) {
                    ArrayAdapter<Restaurant> adapter = new ArrayAdapter<>(
                            requireContext(),
                            android.R.layout.simple_spinner_item,
                            result
                    );
                    adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
                    spinnerRestaurants.setAdapter(adapter);
                }
            }
        });
    }

    private void showDatePicker() {
        DatePickerDialog datePickerDialog = new DatePickerDialog(
                requireContext(),
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
                requireContext(),
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
        String dateFormat = "dd/MM/yyyy";
        SimpleDateFormat sdf = new SimpleDateFormat(dateFormat, Locale.getDefault());
        editDate.setText(sdf.format(calendar.getTime()));
    }

    private void updateTimeEditText() {
        String timeFormat = "HH:mm";
        SimpleDateFormat sdf = new SimpleDateFormat(timeFormat, Locale.getDefault());
        editTime.setText(sdf.format(calendar.getTime()));
    }

    private void submitReservation() {
        // Kiểm tra lại customerId
        if (customerId == -1) {
            showLoginRequired();
            return;
        }

        // Validate input
        if (spinnerRestaurants.getSelectedItem() == null) {
            Toast.makeText(requireContext(), "Please select restaurant", Toast.LENGTH_SHORT).show();
            return;
        }

        String dateStr = editDate.getText().toString().trim();
        String timeStr = editTime.getText().toString().trim();
        String quantityStr = editQuantity.getText().toString().trim();
        String noteStr = editNote.getText().toString().trim();

        if (dateStr.isEmpty() || timeStr.isEmpty()) {
            Toast.makeText(requireContext(), "Please select date and time", Toast.LENGTH_SHORT).show();
            return;
        }

        if (quantityStr.isEmpty()) {
            Toast.makeText(requireContext(), "Please enter quantity of people", Toast.LENGTH_SHORT).show();
            return;
        }

        try {
            // Parse và định dạng ngày giờ
            SimpleDateFormat dateFormat = new SimpleDateFormat("dd/MM/yyyy", Locale.getDefault());
            SimpleDateFormat timeFormat = new SimpleDateFormat("HH:mm", Locale.getDefault());
            Date date = dateFormat.parse(dateStr);
            Date time = timeFormat.parse(timeStr);

            Calendar combinedCalendar = Calendar.getInstance();
            combinedCalendar.setTime(date);

            Calendar timeCalendar = Calendar.getInstance();
            timeCalendar.setTime(time);

            combinedCalendar.set(Calendar.HOUR_OF_DAY, timeCalendar.get(Calendar.HOUR_OF_DAY));
            combinedCalendar.set(Calendar.MINUTE, timeCalendar.get(Calendar.MINUTE));

            SimpleDateFormat sqlFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss.SSSSSSS", Locale.getDefault());
            String sqlDateTime = sqlFormat.format(combinedCalendar.getTime());

            Restaurant selectedRestaurant = (Restaurant) spinnerRestaurants.getSelectedItem();
            int restaurantId = selectedRestaurant.getId();

            // Log thông tin trước khi gửi
            Log.d("Reservation", "Submitting reservation - CustomerID: " + customerId
                    + ", RestaurantID: " + restaurantId
                    + ", DateTime: " + sqlDateTime);

            databaseHelper.createReservation(customerId, restaurantId, sqlDateTime, quantityStr, noteStr,
                    new DatabaseHelper.OnCreateReservationCallback() {
                        @Override
                        public void onResult(boolean success, int reservationId) {
                            if (success) {
                                Toast.makeText(requireContext(),
                                        "Make reservation successful! Table reservation code: " + reservationId,
                                        Toast.LENGTH_LONG).show();
                                resetForm();
                            } else {
                                Toast.makeText(requireContext(),
                                        "Make reservation fail. Please try again.",
                                        Toast.LENGTH_SHORT).show();
                            }
                        }
                    });
        } catch (Exception e) {
            Toast.makeText(requireContext(), "Lỗi khi xử lý đặt bàn: " + e.getMessage(), Toast.LENGTH_SHORT).show();
            Log.e("Reservation", "Error processing reservation", e);
        }
    }

    private void resetForm() {
        editDate.setText("");
        editTime.setText("");
        editQuantity.setText("");
        editNote.setText("");
    }
}