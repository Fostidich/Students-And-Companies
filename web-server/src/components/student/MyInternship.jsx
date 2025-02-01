import Cookies from 'js-cookie';
import {useState, useEffect} from "react";
import {getErrorMessage} from "../../utils/errorUtils.js";
const API_SERVER_URL = window.env?.VITE_API_SERVER_URL || 'http://localhost:5000';

function MyInternship() {
    const [status, setStatus] = useState({
        loading: true,
        error: null,
        hasInternship: false
    });
    const [internship, setInternship] = useState(null);
    const [advertisement, setAdvertisement] = useState(null);

    useEffect(() => {
        fetchData();
    }, []);

    const fetchData = async () => {
        try {
            const internshipData = await getInternship();
            if (!internshipData) {
                setStatus({loading: false, error: null, hasInternship: false});
                return;
            }

            const advData = await getInternshipDetails(internshipData.advertisementId);
            if (!advData) {
                setStatus({loading: false, error: "Failed to load advertisement details", hasInternship: true});
                return;
            }

            setInternship(internshipData);
            setAdvertisement(advData);
            setStatus({loading: false, error: null, hasInternship: true});

        } catch (error) {
            setStatus({loading: false, error: error.message, hasInternship: false});
        }
    };

    const getInternship = async () => {
        const authData = JSON.parse(Cookies.get('authData'));
        const response = await fetch(`${API_SERVER_URL}/api/internship`, {
            headers: {
                'Authorization': `Bearer ${authData.token}`,
            },
        });

        if (!response.ok) {
            if (response.status === 404) return null;
            throw new Error(await getErrorMessage(response));
        }

        return await response.json();
    }

    const getInternshipDetails = async (advertisementId) => {
        const authData = JSON.parse(Cookies.get('authData'));
        const response = await fetch(`${API_SERVER_URL}/api/recommendation/advertisements/${advertisementId}`, {
            headers: {
                'Authorization': `Bearer ${authData.token}`,
            },
        });

        if (!response.ok) return null;
        return await response.json();
    }

    const formatDate = (dateString) => {
        return new Date(dateString).toLocaleDateString();
    };

    if (status.loading) return <div>Loading...</div>;

    if (status.error) {
        return (
            <div className="bg-white rounded-lg border p-4 mb-4">
                <p className="text-red-500">{status.error}</p>
            </div>
        );
    }

    if (!status.hasInternship) {
        return (
            <div className="bg-white rounded-lg border p-4 mb-4">
                <p>No ongoing internship.</p>
            </div>
        );
    }

    return (
        <div className="bg-white rounded-lg p-4 mb-4">
            <div className="space-y-2">
                <h1 className="text-xl font-semibold">Your ongoing internship</h1>
                <p><strong>Advertisement Name:</strong> {advertisement?.name}</p>
                <p><strong>Advertisement Description:</strong> {advertisement?.description}</p>
                <p><strong>Advertisement Duration:</strong> {advertisement?.duration} months</p>
                <p><strong>Internship Start Date:</strong> {formatDate(internship?.startDate)}</p>
                <p><strong>Internship End Date:</strong> {formatDate(internship?.endDate)}</p>
            </div>
        </div>
    );
}

export default MyInternship;