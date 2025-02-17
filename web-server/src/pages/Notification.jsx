import { useState, useEffect } from "react";
import Cookies from "js-cookie";
import { getErrorMessage } from "../utils/errorUtils.js";
import StudentNotification from "../components/student/StudentNotification.jsx";
const API_SERVER_URL =
  window.env?.VITE_API_SERVER_URL || "http://localhost:5000";

function Notification() {
  const [notification, setNotification] = useState([]);
  const [status, setStatus] = useState({
    loading: true,
    error: null,
  });

  useEffect(() => {
    getNotification();
  }, []);

  const getNotification = async () => {
    try {
      setStatus({ loading: true, error: null });
      setNotification([]);

      const authData = JSON.parse(Cookies.get("authData"));
      const response = await fetch(`${API_SERVER_URL}/api/notification`, {
        headers: {
          Authorization: `Bearer ${authData.token}`,
        },
      });

      if (response.status === 404) {
        setStatus({ loading: false, error: null });
        return;
      }

      if (!response.ok) {
        const message = await getErrorMessage(response);
        setStatus({ loading: false, error: message });
        return;
      }

      const data = await response.json();
      setNotification(data);
      setStatus({ loading: false, error: null });
    } catch (error) {
      setStatus({ loading: false, error: error.message });
    }
  };

  const handleNotificationDelete = (notificationId) => {
    setNotification((prev) =>
      prev.filter((not) => not.studentNotificationId !== notificationId),
    );
  };

  if (status.loading) return <div className="p-8">Loading...</div>;

  if (status.error) {
    return <div className="p-8 text-red-500">Error: {status.error}</div>;
  }

  if (notification.length === 0) {
    return (
      <div className="p-8 flex flex-col items-center justify-center">
        No notifications available
      </div>
    );
  }

  return (
    <div className="p-8">
      {notification.map((not) => (
        <StudentNotification
          key={not.studentNotificationId}
          notification={not}
          onDelete={handleNotificationDelete}
        />
      ))}
    </div>
  );
}

export default Notification;
