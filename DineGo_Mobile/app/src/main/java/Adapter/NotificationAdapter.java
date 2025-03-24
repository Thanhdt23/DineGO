package Adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;

import com.example.dinego_mobile.R;

import java.util.List;

import Models.Notification;

public class NotificationAdapter extends RecyclerView.Adapter<NotificationAdapter.ViewHolder> {
    private List<Notification> notificationList;
    private Context context;

    public NotificationAdapter(Context context, List<Notification> notificationList) {
        this.context = context;
        this.notificationList = notificationList;
    }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.item_notification, parent, false);
        return new ViewHolder(view);
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        Notification notification = notificationList.get(position);

        holder.titleTextView.setText(notification.getNotiTitle());
        holder.contentTextView.setText(notification.getNotiContent());
        holder.dateTextView.setText(notification.getNotiDate());

        // Nếu notification có trạng thái "đã đọc", hiển thị icon dấu tích xanh
        if (notification.getIsRead()) {
            holder.statusIcon.setVisibility(View.VISIBLE);
        } else {
            holder.statusIcon.setVisibility(View.GONE);
        }

        // Nếu có loại thông báo (ví dụ: khuyến mãi, cảnh báo...), thay đổi icon
        if (notification.getNotiType().equals("promotion")) {
            holder.notiIcon.setImageResource(R.drawable.ic_promotion); // Icon khuyến mãi
        } else if (notification.getNotiType().equals("warning")) {
            holder.notiIcon.setImageResource(R.drawable.ic_warning); // Icon cảnh báo
        } else {
            holder.notiIcon.setImageResource(R.drawable.ic_notification); // Icon mặc định
        }
    }

    @Override
    public int getItemCount() {
        return notificationList.size();
    }

    public static class ViewHolder extends RecyclerView.ViewHolder {
        TextView titleTextView, contentTextView, dateTextView;
        ImageView notiIcon, statusIcon;

        public ViewHolder(View itemView) {
            super(itemView);
            titleTextView = itemView.findViewById(R.id.noti_title);
            contentTextView = itemView.findViewById(R.id.noti_content);
            dateTextView = itemView.findViewById(R.id.noti_date);
            notiIcon = itemView.findViewById(R.id.noti_icon);
            statusIcon = itemView.findViewById(R.id.noti_status);
        }
    }
}
