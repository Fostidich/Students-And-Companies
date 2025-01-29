import Cookies from 'js-cookie';
import {useState, useEffect} from "react";
import {getErrorMessage} from "../../utils/errorUtils.js";
const API_SERVER_URL = window.env?.VITE_API_SERVER_URL || 'http://localhost:5000';

function MyInternship() {
    const [errorMessage, setErrorMessage] = useState('');
    const [internship, setInternship] = useState({
        internshipId: 0,
        createdAt: '',
        studentId: 0,
        companyId: 0,
        advertisementId: 0,
        startDate: '',
        endDate: '',
    });
    const [advertisement, setAdvertisement] = useState({
        advertisementId: 0,
        name: '',
        createdAt: '',
        companyId: 0,
        description: '',
        duration: 0,
        spots: 0,
        available: 0,
        open: false,
        questionnaire: '',
    });

    useEffect(() => {
        const fetchData = async () => {
            try {
                const internshipData = await getInternship();
                if (internshipData?.advertisementId) {
                    await getInternshipDetails(internshipData.advertisementId);
                }
            } catch (error) {
                console.error('Error in fetchData:', error);
            }
        }
        fetchData();
    }, []);

    const getInternship = async () => {
        try {
            const authData = JSON.parse(Cookies.get('authData'));
            const response = await fetch(`${API_SERVER_URL}/api/internship`, {
                headers: {
                    'Authorization': `Bearer ${authData.token}`,
                },
            });

            if (!response.ok) {
                const message = await getErrorMessage(response);
                setErrorMessage(message);
                throw new Error(message);
            }

            const data = await response.json();
            setInternship(data);
            return data;

        } catch (error) {
            console.error('Error fetching internship:', error);
            setErrorMessage(error.message);
            throw error;
        }
    }

    const getInternshipDetails = async (advertisementId) => {
        try {
            const authData = JSON.parse(Cookies.get('authData'));
            const response = await fetch(`${API_SERVER_URL}/api/recommendation/advertisements/${advertisementId}`, {
                headers: {
                    'Authorization': `Bearer ${authData.token}`,
                },
            });

            if (!response.ok) {
                const message = await getErrorMessage(response);
                setErrorMessage(message);
                throw new Error(message);
            }

            const data = await response.json();
            setAdvertisement(data);

        } catch (error) {
            console.error('Error fetching advertisement details:', error);
            setErrorMessage(error.message);
            throw error;
        }
    }

    const ongoingInternship = internship.advertisementId !== 0;

    return (
        <div>
            {ongoingInternship ? (
                <div className="bg-white rounded-lg border p-4 mb-4">
                    <div className="space-y-2">
                        <h1 className="text-xl font-semibold">Your ongoing internship </h1>
                        <p><strong>Advertisement Name:</strong> {advertisement.name}</p>
                        <p><strong>Advertisement Description:</strong> {advertisement.description}</p>
                        <p><strong>Advertisement Duration:</strong> {advertisement.duration} months</p>
                        <p><strong>Internship Start Date:</strong> {internship.startDate}</p>
                        <p><strong>Internship End Date:</strong> {internship.endDate}</p>
                    </div>
                </div>
            ) : (
                <div className="bg-white rounded-lg border p-4 mb-4">
                    <p>{errorMessage || 'No ongoing internship.'}</p>
                </div>
            )}
        </div>
    );
}

export default MyInternship;