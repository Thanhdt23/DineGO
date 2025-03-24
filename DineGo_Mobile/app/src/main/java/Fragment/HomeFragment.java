package Fragment;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.SearchView;
import android.widget.TextView;

import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentTransaction;
import androidx.recyclerview.widget.GridLayoutManager;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.example.dinego_mobile.LoginActivity;
import com.example.dinego_mobile.R;

import java.util.ArrayList;
import java.util.List;

import Adapter.RestaurantAdapter;
import Data.DatabaseHelper;
import Models.Restaurant;

public class HomeFragment extends Fragment {
    TextView tvName;
    private RecyclerView recyclerView;
    private RestaurantAdapter adapter;
    private SearchView search;
    private List<Restaurant> restaurantList = new ArrayList<>();


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_home, container, false);

        tvName = view.findViewById(R.id.tvName); // Sử dụng `view` để tìm TextView
        search = view.findViewById(R.id.searchView);

        // Xử lý sự kiện khi bấm "Forgot your password?"
        search.setOnClickListener(v -> {
            RestaurantFragment restaurantFragment = new RestaurantFragment();
            FragmentTransaction transaction = getParentFragmentManager().beginTransaction();
            transaction.replace(R.id.fragment_container, restaurantFragment); // ID của container chứa Fragment
            transaction.addToBackStack(null); // Thêm vào stack để có thể quay lại
            transaction.commit();
        });

        // Lấy username từ session
        SharedPreferences sharedPreferences = requireActivity().getSharedPreferences("UserSession", Context.MODE_PRIVATE);
        String username = sharedPreferences.getString("USERNAME", "Guest");
        // Hiển thị username lên TextView
        tvName.setText(username);

        recyclerView = view.findViewById(R.id.recyclerViewRestaurants);
        recyclerView.setLayoutManager(new GridLayoutManager(getContext(), 2));

        adapter = new RestaurantAdapter(getContext(), restaurantList);
        recyclerView.setAdapter(adapter);

        loadRestaurants();

        return view; // Trả về View sau khi thiết lập xong
    }

    private void loadRestaurants() {
        DatabaseHelper databaseHelper = new DatabaseHelper();
        databaseHelper.getRestaurants(restaurants -> {
            if (restaurants != null) {
                restaurantList.clear();
                restaurantList.addAll(restaurants);
                adapter.notifyDataSetChanged();
            }
        });
    }
}

