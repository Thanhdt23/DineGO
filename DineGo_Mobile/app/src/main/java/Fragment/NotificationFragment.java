package Fragment;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.example.dinego_mobile.R;

import java.util.ArrayList;
import java.util.List;

import Adapter.NotificationAdapter;
import Data.DatabaseHelper;
import Models.Notification;

public class NotificationFragment extends Fragment {
    private RecyclerView recyclerView;
    private NotificationAdapter adapter;
    private List<Notification> notificationList = new ArrayList<>();

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_notification, container, false); // Sửa layout

        recyclerView = view.findViewById(R.id.recyclerViewNotifications); // Đổi ID cho đúng
        recyclerView.setLayoutManager(new LinearLayoutManager(getContext()));

        adapter = new NotificationAdapter(getContext(), notificationList); // Dùng NotificationAdapter
        recyclerView.setAdapter(adapter);

        loadNotifications();  // Gọi phương thức để tải dữ liệu từ database

        return view;
    }

    private void loadNotifications() {
        DatabaseHelper databaseHelper = new DatabaseHelper();
        databaseHelper.getNotifications(notifications -> { // Đổi từ getRestaurants() -> getNotifications()
            if (notifications != null) {
                notificationList.clear();
                notificationList.addAll(notifications);
                adapter.notifyDataSetChanged();
            }
        });
    }
}
