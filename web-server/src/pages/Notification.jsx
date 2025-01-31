import { useState, useEffect } from "react";
import Cookies from "js-cookie";
import {getErrorMessage} from "../utils/errorUtils.js";
import StudentNotification from "../components/student/StudentNotification.jsx";
const API_SERVER_URL = window.env?.VITE_API_SERVER_URL || 'http://localhost:5000';
function Notification() {
    const [notification, setNotification] = useState([]);

    useEffect(() => {
        const fetchData = async () => {
            await getNotification();
        }
        fetchData();
    },[]);

    const getNotification = async () => {
        setNotification([]);
        const authData = JSON.parse(Cookies.get('authData'));
        const response = await fetch(`${API_SERVER_URL}/api/notification`, {
            headers: {
                'Authorization': `Bearer ${authData.token}`,
            }
        });
        if (response.status === 200) {
            const data = await response.json();
           setNotification(data);
        } else {
            const message = await getErrorMessage(response);
            console.error('Error fetching notifications:', message);
        }
    }

    return (
        <div className="p-8">
            {notification.map((not) =>
                <StudentNotification
                    key={not.studentNotificationId}
                    notification={not}
                />)}
        </div>
    );
}

export default Notification;