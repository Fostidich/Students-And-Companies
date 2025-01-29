import { useState } from 'react';
import PropTypes from 'prop-types';
import Cookies from 'js-cookie';
import {getErrorMessage} from "../../utils/errorUtils.js";
import StudentInternship from "./StudentInternship.jsx";
import RecommendedStudent from "./RecommendedStudent.jsx";
const API_SERVER_URL = window.env?.VITE_API_SERVER_URL || 'http://localhost:5000';

function AdvertisementCompany({ advertisement }) {
    const [showDetails, setShowDetails] = useState(false);
    const authData = JSON.parse(Cookies.get('authData'));
    const [errorMessage, setErrorMessage] = useState('');
    const [internship] = useState(
        {
            internshipId: 0,
            createdAt: '',
            studentId: 0,
            companyId: 0,
            advertisementId: 0,
            startDate: '',
            endDate: '',
        }
    );
    const [internshipList, setInternshipList] = useState([internship]);
    const [profileStudent] = useState({
        studentId: 0,
        username: '',
        email: '',
        bio: '',
        name: '',
        surname: '',
        university: '',
        courseOfStudy: '',
        gender: '',
        birthDate: '',
    });
    const [recommendedStudents, setRecommendedStudents] = useState([profileStudent]);



    const formatDate = (dateString) => {
        return new Date(dateString).toLocaleDateString();
    };
    const getInternshipList = async () => {
        const response = await fetch(`${API_SERVER_URL}/api/internship/advertisements/${advertisement.advertisementId}`, {
            headers: {
                'Authorization': `Bearer ${authData.token}`,
            },
        });
        if (response.status === 200) {
            const data = await response.json();
            setInternshipList(data);
        }else {
            const message = await getErrorMessage(response);
            setErrorMessage(message);
            console.error('Error checking advertisement details:', errorMessage);
        }
    }

    const getRecommendedStudents = async () => {
        try {
            const response = await fetch(`${API_SERVER_URL}/api/recommendation/candidates/advertisements/${advertisement.advertisementId}`, {
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
            // Modifica qui: estrai l'array effettivo dalla risposta
            const students = data.students || data; // Adatta in base alla struttura reale della risposta
            setRecommendedStudents(students);

        } catch (error) {
            console.error('Error fetching recommended students:', error);
            setErrorMessage(error.message);
            // Resetta la lista in caso di errore
            setRecommendedStudents([]);
        }
    }




    return (
        <div className="bg-white rounded-lg border p-4 mb-4">
            <div className="flex justify-between items-start">
                <div>
                    <p className="text-xl font-semibold mb-2">{advertisement.description}</p>
                    <div className="flex gap-4">
                        <p>Duration: {advertisement.duration} months</p>
                        <p>Spots: {advertisement.spots}</p>
                        <p>Available: {advertisement.available}</p>
                    </div>
                </div>
                <button
                    onClick={() => {
                        setShowDetails(true);
                        getInternshipList();
                        getRecommendedStudents();
                    }}
                    className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600"
                >
                    View Details
                </button>
            </div>

            {showDetails && (
                <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
                    <div className="bg-white p-6 rounded-lg w-2/3 h-auto relative">
                        <button
                            onClick={() => setShowDetails(false)}
                            className="absolute top-2 right-2 text-gray-600 hover:text-gray-800"
                        >
                            ✕
                        </button>
                        <h2 className="text-xl font-bold mb-4">Advertisement Details</h2>
                        <div className=" border p-4 rounded-md space-y-2">
                            <h3 className="text-lg font-semibold">
                                {advertisement.name}
                            </h3>
                            <p className="text-sm"><strong>Description:</strong> {advertisement.description}</p>
                            <p className="text-sm"><strong>Questionnaire:</strong> {advertisement.questionnaire}</p>
                            <div className="mt-2 border-t pt-2">
                                <p className="text-xs text-gray-600"><strong>ADV ID:</strong> {advertisement.advertisementId}</p>
                                <p className="text-xs text-gray-600"><strong>Company ID:</strong> {advertisement.companyId}</p>
                                <p className="text-xs text-gray-600"><strong>Duration:</strong> {advertisement.duration}</p>
                                <p className="text-xs text-gray-600"><strong>Total spots:</strong> {advertisement.spots}</p>
                                <p className="text-xs text-gray-600"><strong>Available spots:</strong> {advertisement.available}</p>
                                <p className="text-xs text-gray-600"><strong>Stutus:</strong> {advertisement.open ? 'Open' : 'Closed'}</p>
                                <p className="text-xs text-gray-600"><strong>Created:</strong> {formatDate(advertisement.createdAt)}</p>
                            </div>
                        </div>
                        <div className=" p-2 rounded-md mt-4 border">
                            <h2 className="text-xl font-bold mb-4">Ongoing internships</h2>
                            <div className="shadow-md ">
                                {
                                    internshipList.length > 0 ? (
                                        internshipList.map((internship) =>
                                            <StudentInternship
                                                key={internship.internshipId}
                                                internship={internship}
                                            />)
                                    ):(
                                        <p>No student applied yet</p>
                                    )
                                }
                            </div>
                        </div>
                        <div className=" p-2 rounded-md mt-4 border">
                            <h2 className="text-xl font-bold mb-4">Let me suggest some students</h2>
                            <div className="shadow-md ">
                                {recommendedStudents.length > 0 ? (
                                    recommendedStudents.map((student) =>
                                        <RecommendedStudent
                                            key={student.studentId}
                                            student={student}
                                        />)
                                ):(
                                    <h1>No student recommended yet</h1>
                                )}
                            </div>
                        </div>


                    </div>
                </div>
            )}
        </div>
    );
}

AdvertisementCompany.propTypes = {
    advertisement: PropTypes.shape({
        advertisementId: PropTypes.number.isRequired,
        name: PropTypes.string.isRequired,
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

export default AdvertisementCompany;