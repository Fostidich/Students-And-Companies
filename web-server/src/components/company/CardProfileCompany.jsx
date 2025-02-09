import { useState, useEffect } from 'react';
import Cookies from 'js-cookie';
import {getErrorMessage} from "../../utils/errorUtils.js";

const API_SERVER_URL = window.env?.VITE_API_SERVER_URL || 'http://localhost:5000';

function CardProfileCompany() {
    const [isEditDialogOpen, setIsEditDialogOpen] = useState(false);
    const [feedbackMessage, setFeedbackMessage] = useState({ type: '', message: '' });
    const [profileData, setProfileData] = useState({
        username: '',
        email: '',
        bio: '',
        headquarter: '',
        fiscalCode: '',
        vatNumber: ''
    });

    // Form state per i dati modificabili
    const [formData, setFormData] = useState({
        username: '',
        email: '',
        bio: '',
        headquarter: '',
        fiscalCode: '',
        vatNumber: ''
    });
    const [password, setPassword] = useState('');
    const [errorMessage, setErrorMessage] = useState('');
    // Stato per i dati originali per il confronto
    const [originalData, setOriginalData] = useState({});

    const showFeedback = (type, message) => {
        setFeedbackMessage({ type, message });
        setTimeout(() => setFeedbackMessage({ type: '', message: '' }), 3000);
    };

    const downloadInformations = async () => {
        try {
            const authData = JSON.parse(Cookies.get('authData'));
            const response = await fetch(`${API_SERVER_URL}/api/profile/company`, {
                headers: {
                    'Authorization': `Bearer ${authData.token}`,
                },
            });

            if (response.status === 200) {
                const data = await response.json();
                setProfileData(data);
                setFormData(data);
                setOriginalData(data);
            } else {
                showFeedback('error', 'Could not load profile information. Please try again.');
            }
        } catch (error) {
            console.error('Error checking profile informations:', error);
            showFeedback('error', 'Could not load profile information. Please try again.');
        }
    };

    useEffect(() => {
        downloadInformations();
    }, []);

    const getChangedFields = () => {
        const changedFields = {};
        Object.keys(formData).forEach(key => {
            // Verifica se il campo è stato modificato rispetto all'originale
            if (formData[key] !== originalData[key]) {
                changedFields[key] = formData[key];
            }
        });
        if (password !== '') {
            changedFields.password = password;
        }
        return changedFields;
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const authData = JSON.parse(Cookies.get('authData'));

            // Ottieni solo i campi modificati
            const changedFields = getChangedFields();

            // Se non ci sono modifiche, non inviare la richiesta
            if (Object.keys(changedFields).length === 0 && password === '') {
                showFeedback('error', 'No changes detected');
                return;
            }

            const response = await fetch(`${API_SERVER_URL}/api/profile/company`, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${authData.token}`,
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(changedFields),
            });

            if (response.status === 200) {
                showFeedback('success', 'Profile updated successfully!');
                await downloadInformations();
                setIsEditDialogOpen(false);
                setPassword('');
            } else {
                setErrorMessage(await getErrorMessage(response));
                console.error('Error updating profile:', response.status);
                showFeedback('error', 'Could not update profile. Please try again.');
            }
        } catch (error) {
            console.error('Error updating profile:', error);
            showFeedback('error', 'Could not update profile. Please try again.');
        }
    };

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        if (name === 'password') {
            setPassword(value);
            return;
        }
        setFormData(prev => ({
            ...prev,
            [name]: value
        }));
    };

    return (
        <div>
            {/* Feedback Message */}
            {feedbackMessage.message && (
                <div className={`fixed top-4 right-4 p-4 rounded-lg shadow-lg transition-all duration-500 ${
                    feedbackMessage.type === 'success'
                        ? 'bg-green-100 border border-green-400 text-green-700'
                        : 'bg-red-100 border border-red-400 text-red-700'
                }`}>
                    {feedbackMessage.message}
                </div>
            )}

            <div className="bg-white flex flex-col rounded-xl p-6 border items-center w-full h-full">
                <div className="flex flex-col pb-4">
                    <img
                        className="w-24 h-24 border rounded-full"
                        src="https://static.vecteezy.com/system/resources/previews/020/911/733/non_2x/profile-icon-avatar-icon-user-icon-person-icon-free-png.png"
                        alt="Profile"
                    />
                </div>
                <div className="flex flex-col items-center gap-2">
                    <p className="text-lg font-bold">Username: <span className="font-light">{profileData.username}</span></p>
                    <p className="text-lg font-bold">Email: <span className="font-light">{profileData.email}</span></p>
                    <p className="text-lg font-bold">Head quarter: <span className="font-light">{profileData.headquarter}</span></p>
                    <p className="text-lg font-bold">Fiscal code: <span className="font-light">{profileData.fiscalCode}</span></p>
                    <p className="text-lg font-bold">Vat number: <span className="font-light">{profileData.vatNumber}</span></p>
                    <p className="text-lg font-bold">Your vision: <span className="font-light">{profileData.bio}</span></p>
                </div>
                <button
                    className="mt-4 px-4 py-2 border rounded-lg hover:bg-gray-50 transition-colors"
                    onClick={() => setIsEditDialogOpen(true)}
                >
                    Edit Profile
                </button>
            </div>

            {/* Edit Modal */}
            {isEditDialogOpen && (
                <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4">
                    <div className="bg-white rounded-lg p-6 max-w-md w-full max-h-[90vh] overflow-y-auto">
                        <div className="flex justify-between items-center mb-4">
                            <h2 className="text-xl font-bold">Edit Company Profile</h2>
                            <button
                                onClick={() => setIsEditDialogOpen(false)}
                                className="text-gray-500 hover:text-gray-700"
                            >
                                ✕
                            </button>
                        </div>
                        <form onSubmit={handleSubmit} className="space-y-4">
                            <div className="space-y-2">
                                <label className="block text-sm font-medium">Username</label>
                                <input
                                    type="text"
                                    name="username"
                                    value={formData.username}
                                    onChange={handleInputChange}
                                    className="w-full border rounded-lg p-2"
                                    placeholder="Username"
                                />
                            </div>
                            <div className="space-y-2">
                                <label className="block text-sm font-medium">Email</label>
                                <input
                                    type="email"
                                    name="email"
                                    value={formData.email}
                                    onChange={handleInputChange}
                                    className="w-full border rounded-lg p-2"
                                    placeholder="Email"
                                />
                            </div>
                            <div className="space-y-2">
                                <label className="block text-sm font-medium">Password</label>
                                <input
                                    type="password"
                                    name="password"
                                    value={password}
                                    onChange={handleInputChange}
                                    className="w-full border rounded-lg p-2"
                                    placeholder="Password"
                                />
                            </div>
                            <div className="space-y-2">
                                <label className="block text-sm font-medium">Head Quarter</label>
                                <input
                                    type="text"
                                    name="headquarter"
                                    value={formData.headquarter}
                                    onChange={handleInputChange}
                                    className="w-full border rounded-lg p-2"
                                    placeholder="Head Quarter"
                                />
                            </div>
                            <div className="space-y-2">
                                <label className="block text-sm font-medium">Fiscal Code</label>
                                <input
                                    type="text"
                                    name="fiscalCode"
                                    value={formData.fiscalCode}
                                    onChange={handleInputChange}
                                    className="w-full border rounded-lg p-2"
                                    placeholder="Fiscal Code"
                                />
                            </div>
                            <div className="space-y-2">
                                <label className="block text-sm font-medium">VAT Number</label>
                                <input
                                    type="text"
                                    name="vatNumber"
                                    value={formData.vatNumber}
                                    onChange={handleInputChange}
                                    className="w-full border rounded-lg p-2"
                                    placeholder="VAT Number"
                                />
                            </div>
                            <div className="space-y-2">
                                <label className="block text-sm font-medium">Company Vision</label>
                                <textarea
                                    name="bio"
                                    value={formData.bio}
                                    onChange={handleInputChange}
                                    className="w-full border rounded-lg p-2 h-24 resize-none"
                                    placeholder="Write your company vision"
                                />
                            </div>
                            <div className="flex justify-end gap-2 pt-4">
                                <button
                                    type="button"
                                    onClick={() => setIsEditDialogOpen(false)}
                                    className="px-4 py-2 border rounded-lg hover:bg-gray-50 transition-colors"
                                >
                                    Cancel
                                </button>
                                <button
                                    type="submit"
                                    className="px-4 py-2 bg-blue-500 text-white rounded-lg hover:bg-blue-600 transition-colors"
                                >
                                    Save Changes
                                </button>
                            </div>
                        </form>
                        {errorMessage && <div className="text-red-500 mb-4">{errorMessage}</div>}
                    </div>
                </div>
            )}
        </div>
    );
}

export default CardProfileCompany;