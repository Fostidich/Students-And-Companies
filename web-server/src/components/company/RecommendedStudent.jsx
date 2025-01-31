import PropTypes from "prop-types";
import { useState } from "react";
import Cookies from "js-cookie";
const API_SERVER_URL = window.env?.VITE_API_SERVER_URL || 'http://localhost:5000';


function RecommendedStudent({ student }) {
    const [isLoading, setIsLoading] = useState(false);
    const [isDownloading, setIsDownloading] = useState(false);

    const handleNotify = async () => {
        setIsLoading(true);
        try {
            // Invia la richiesta di notifica

        } catch {
            // L'errore è già gestito nel componente padre
        } finally {
            setIsLoading(false);
        }
    };

    const getStudentCV = async () => {
        setIsDownloading(true);
        const authData = JSON.parse(Cookies.get('authData'));
        const response = await fetch(`${API_SERVER_URL}/api/profile/cv/${student.studentId}`, {
            headers: {
                'Authorization': `Bearer ${authData.token}`,
            },
        });
        if (response.status === 200) {

            // Converti la risposta in blob
            const blob = await response.blob();

            // Crea un URL temporaneo per il download
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;

            // Estrai il filename dall'header o usa un default
            const contentDisposition = response.headers.get('content-disposition');
            const filename = contentDisposition
                ? contentDisposition.split('filename=')[1].replace(/"/g, '')
                : `CV_studentId${student.studentId}.pdf`;

            a.download = filename;
            document.body.appendChild(a);
            a.click();

            // Pulizia
            window.URL.revokeObjectURL(url);
            document.body.removeChild(a);

        }
        setIsDownloading(false);

    }

    return (
        <div className="bg-white p-4 rounded-lg shadow-md mb-4">
            <div className="space-y-2">
                <h3 className="text-lg font-semibold">
                    {student.name} {student.surname}
                </h3>
                <p className="text-sm"><strong>University:</strong> {student.university}</p>
                <p className="text-sm"><strong>Course:</strong> {student.courseOfStudy}</p>
                <p className="text-sm"><strong>Bio:</strong> {student.bio}</p>
                <div className="mt-2 border-t pt-2">
                    <p className="text-xs text-gray-600">{student.email}</p>
                    <p className="text-xs text-gray-600">Username: {student.username}</p>
                </div>
                <div>
                    <button
                        onClick={getStudentCV}
                        disabled={isDownloading}
                        className="mt-3 m-1 bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 disabled:bg-gray-400"
                    >
                        {isDownloading ? 'Downloading...' : 'Download CV'}
                    </button>
                    <button
                        onClick={handleNotify}
                        disabled={isLoading}
                        className="mt-3 bg-green-500 text-white px-4 py-2 rounded hover:bg-green-600 disabled:bg-gray-400"
                    >
                        {isLoading ? 'Sending...' : 'Request to Apply'}
                    </button>
                </div>

            </div>
        </div>
    );
}

RecommendedStudent.propTypes = {
    student: PropTypes.shape({
        studentId: PropTypes.number.isRequired,
        username: PropTypes.string.isRequired,
        email: PropTypes.string.isRequired,
        bio: PropTypes.string.isRequired,
        name: PropTypes.string.isRequired,
        surname: PropTypes.string.isRequired,
        university: PropTypes.string.isRequired,
        courseOfStudy: PropTypes.string.isRequired,
        gender: PropTypes.string.isRequired,
        birthDate: PropTypes.string.isRequired
    }).isRequired
};
export default RecommendedStudent;