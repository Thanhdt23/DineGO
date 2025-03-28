    package Fragment;

    import android.os.Bundle;
    import android.view.LayoutInflater;
    import android.view.View;
    import android.view.ViewGroup;
    import android.widget.TextView;

    import androidx.annotation.NonNull;
    import androidx.fragment.app.Fragment;
    import androidx.recyclerview.widget.LinearLayoutManager;
    import androidx.recyclerview.widget.RecyclerView;

    import com.example.dinego_mobile.R;

    import java.util.ArrayList;
    import java.util.List;

    import Adapter.ReservationAdapter;
    import Data.DatabaseHelper;
    import Models.Reservation;

    public class ReservationFragment extends Fragment {
        private RecyclerView recyclerView;
        private ReservationAdapter adapter;
        private TextView tvEmptyMessage;
        private List<Reservation> reservationList = new ArrayList<>();
        private DatabaseHelper databaseHelper;

        @Override
        public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
            View view = inflater.inflate(R.layout.fragment_reservation, container, false);

            recyclerView = view.findViewById(R.id.recyclerViewReservations);
            tvEmptyMessage = view.findViewById(R.id.tv_empty_message);
            databaseHelper = new DatabaseHelper();

            // Setup RecyclerView
            recyclerView.setLayoutManager(new LinearLayoutManager(getContext()));

            adapter = new ReservationAdapter(getContext(), reservationList, reservation -> {
                // Xử lý khi click vào View Details
                // Có thể mở fragment/activity chi tiết ở đây
            });
            recyclerView.setAdapter(adapter);

            loadReservations();

            return view;
        }

        private void loadReservations() {
            databaseHelper.getReservations(reservations -> {
                if (reservations != null) {
                    reservationList.clear();
                    reservationList.addAll(reservations);
                    adapter.notifyDataSetChanged();

                    // Hiển thị thông báo nếu không có dữ liệu
                    if (reservationList.isEmpty()) {
                        tvEmptyMessage.setVisibility(View.VISIBLE);
                        recyclerView.setVisibility(View.GONE);
                    } else {
                        tvEmptyMessage.setVisibility(View.GONE);
                        recyclerView.setVisibility(View.VISIBLE);
                    }
                }
            });
        }
    }