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

import Adapter.ReservationAdapter;
import Adapter.RestaurantAdapter;
import Data.DatabaseHelper;
import Models.Reservation;

public class ReservationFragment extends Fragment {
    private RecyclerView recyclerView;
    private ReservationAdapter adapter;
    private List<Reservation> reservationList = new ArrayList<>();

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_reservation, container, false);

        recyclerView = view.findViewById(R.id.recyclerViewReservations);
        recyclerView.setLayoutManager(new LinearLayoutManager(getContext()));

        adapter = new ReservationAdapter(getContext(), reservationList);
        recyclerView.setAdapter(adapter);

        loadResservations();  // Gọi phương thức để tải dữ liệu từ database

        return view;
    }

    private void loadResservations() {
        DatabaseHelper databaseHelper = new DatabaseHelper();
        databaseHelper.getReservations(reservations -> {
            if (reservations != null) {
                reservationList.clear();
                reservationList.addAll(reservations);
                adapter.notifyDataSetChanged();
            }
        });
    }
}
