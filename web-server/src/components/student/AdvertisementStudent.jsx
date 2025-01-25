import { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import Cookies from "js-cookie";
const API_SERVER_URL = import.meta.env.VITE_API_SERVER_URL;
function AdvertisementStudent({ advertisement }) {
    const [showDetails, setShowDetails] = useState(false);
    const [profileCompany, setProfileCompany] = useState({
        username: '',
        email: '',
        bio: '',
        headquarter: '',
        fiscalCode: '',
        vatNumber: ''
    });
    const [response, setResponse] = useState('');

    const formatDate = (dateString) => {
        return new Date(dateString).toLocaleDateString();
    };

    useEffect(() => {
        getDetails();
    }, []);

    const getDetails = async () => {
        try {
            const authData = JSON.parse(Cookies.get('authData'));
            const response = await fetch(`${API_SERVER_URL}/api/profile/company/${advertisement.companyId}`, {
                headers: {
                    'Authorization': `Bearer ${authData.token}`,
                },
            });

            if (response.status === 200) {
                const data = await response.json();
                setProfileCompany(data);
            }
        } catch (error) {
            console.error('Error checking advertisement details:', error);
        }
    }


    const handleSubmit = async (e) => {
        e.preventDefault()
        console.log('Applied');
        setShowDetails(false);
    }

    return (
        <div className="bg-white rounded-lg border p-4 mb-4">
            <div className="flex justify-between items-start">
                <div>
                    <h2 className="text-2xl font-bold">{profileCompany.username}</h2>
                    <p className="text-xl font-semibold mb-2">{advertisement.description}</p>
                    <div className="flex gap-4">
                        <p>Duration: {advertisement.duration} months</p>
                        <p>Spots: {advertisement.spots}</p>
                        <p>Available: {advertisement.available}</p>
                    </div>
                </div>
                <button
                    onClick={() => setShowDetails(true)}
                    className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
                >
                    View Details
                </button>
            </div>

            {showDetails && (
                <div className=" fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
                    <div className="bg-white p-6 rounded-lg w-2/3 h-auto relative">
                        <button
                            onClick={() => setShowDetails(false)}
                            className="absolute top-2 right-2 text-gray-600 hover:text-gray-800"
                        >
                            ✕
                        </button>
                        <h2 className="text-xl font-bold mb-4">Advertisement Details</h2>
                        <div className="space-y-2">
                            <p><strong>ADV ID:</strong> {advertisement.advertisementId}</p>
                            <p><strong>Created:</strong> {formatDate(advertisement.createdAt)}</p>
                            <p><strong>Company ID:</strong> {advertisement.companyId}</p>
                            <p><strong>Description:</strong> {advertisement.description}</p>
                            <p><strong>Duration:</strong> {advertisement.duration} months</p>
                            <p><strong>Total Spots:</strong> {advertisement.spots}</p>
                            <p><strong>Available Spots:</strong> {advertisement.available}</p>
                            <p><strong>Status:</strong> {advertisement.open ? 'Open' : 'Closed'}</p>
                        </div>
                        <div className=" items-center justify-center">
                            <form onSubmit={handleSubmit}>
                                <p><strong>Questionnaire: </strong> {advertisement.questionnaire}</p>
                                <input
                                    type="text"
                                    value={response}
                                    onChange={(e) => setResponse(e.target.value)}
                                    className="border rounded p-2 w-full"
                                    required
                                />
                                <button className=" gap-2  p-2 border rounded-md text-white bg-green-500 hover:bg-green-600" type="submit">Apply</button>
                            </form>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
}

AdvertisementStudent.propTypes = {
    advertisement: PropTypes.shape({
        advertisementId: PropTypes.number.isRequired,
        createdAt: PropTypes.string.isRequired,
        companyId: PropTypes.number.isRequired,
        description: PropTypes.string.isRequired,
        duration: PropTypes.number.isRequired,
        spots: PropTypes.number.isRequired,
        available: PropTypes.number.isRequired,
        open: PropTypes.bool.isRequired,
        questionnaire: PropTypes.string.isRequired
    }).isRequired
};

export default AdvertisementStudent;