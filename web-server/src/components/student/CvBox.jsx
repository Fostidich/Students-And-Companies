import { useState, useEffect } from 'react';
import Cookies from 'js-cookie';
const API_SERVER_URL = window.env?.VITE_API_SERVER_URL || 'http://localhost:5000';

function CvBox() {
    const [showCv, setShowCv] = useState(false);
    const [cvUrl, setCvUrl] = useState('');
    const [file, setFile] = useState(null);
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);


    useEffect(() => {
        checkExistingCv();
    }, []);

    const checkExistingCv = async () => {
        try {
            const authData = JSON.parse(Cookies.get('authData'));
            const response = await fetch(`${API_SERVER_URL}/api/profile/cv`, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${authData.token}`,
                },
            });

            if (response.status === 200) {
                const blob = await response.blob();
                const url = URL.createObjectURL(blob);
                setCvUrl(url);
                setShowCv(true);
            } else {
                setShowCv(false);
            }
        } catch (error) {
            console.error('Error checking CV:', error);
            setError('Errore nel controllo del CV esistente');
        }
    };

    const handleFileChange = (e) => {
        const selectedFile = e.target.files[0];

        if (selectedFile) {
            if (selectedFile.type !== 'application/pdf') {
                setError('Per favore seleziona un file PDF');
                setFile(null);
                return;
            }

            if (selectedFile.size > 1 * 1024 * 1024) { // 1MB limit
                setError('Il file non deve superare i 5MB');
                setFile(null);
                return;
            }

            setFile(selectedFile);
            setError('');
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!file) {
            setError('Seleziona un file PDF');
            return;
        }

        setLoading(true);
        setError('');

        try {
            const formData = new FormData();
            formData.append('cv', file);

            const authData = JSON.parse(Cookies.get('authData'));
            const response = await fetch(`${API_SERVER_URL}/api/profile/cv`, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${authData.token}`,
                },
                body: formData,
            });

            if (response.ok) {
                console.log('CV uploaded '+  response.status);
                setShowCv(true);
                setFile(null);
                checkExistingCv(); // Refresh the CV URL
            } else {
                throw new Error(`Upload fallito: ${response.status}`);
            }
        } catch (error) {
            console.error('Error uploading CV:', error);
            setError('Errore nel caricamento del CV');
        } finally {
            setLoading(false);
        }
    };
    const deleteCv = async () => {
        try {
            const authData = JSON.parse(Cookies.get('authData'));
            const response = await fetch(`${API_SERVER_URL}/api/profile/cv/delete`, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${authData.token}`,
                },
            });

            if (response.ok) {
                console.log('CV deleted '+  response.status);
                setShowCv(false);
            } else {
                throw new Error(`Delete fallito: ${response.status}`);
            }
        } catch (error) {
            console.error('Error deleting CV:', error);
            setError('Errore nella cancellazione del CV');
        }

    }

    return (
        <div className="rounded-md flex flex-wrap items-center justify-center">
            {!showCv ? (
                <div className="flex flex-col gap-4">
                    {error && (
                        <div className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded">
                            {error}
                        </div>
                    )}

                    <form onSubmit={handleSubmit} className="flex flex-col gap-4">
                        <div className="flex flex-col gap-2">
                            <label htmlFor="cv-upload" className="font-medium">
                                Upload your CV (PDF, max 5MB)
                            </label>
                            <input
                                id="cv-upload"
                                type="file"
                                accept=".pdf,application/pdf"
                                onChange={handleFileChange}
                                className="border rounded p-2"
                            />
                        </div>

                        <button
                            type="submit"
                            disabled={!file || loading}
                            className="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 disabled:bg-gray-300 disabled:cursor-not-allowed"
                        >
                            {loading ? 'Loading...' : 'Upload your CV'}
                        </button>
                    </form>
                </div>
            ) : (
                <div className="text-center p-4 flex flex-col gap-2">
                    <button
                        onClick={() => window.location.href = cvUrl}
                        className="bg-green-100 border border-green-400 px-4 py-3 rounded text-green-600 font-medium p-6"
                    >
                        Download CV
                    </button>
                    <button
                        onClick={deleteCv}
                        className="bg-red-100 border border-red-400 text-red-700 px-4 py-3 rounded">
                        Delete CV
                    </button>
                </div>
            )}
        </div>
    );
}

export default CvBox;
