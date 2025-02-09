import { useState, useEffect } from 'react';
import Cookies from 'js-cookie';
import {getErrorMessage} from "../../utils/errorUtils.js";
const API_SERVER_URL = window.env?.VITE_API_SERVER_URL || 'http://localhost:5000';

function CardProfileStudent() {
    const [username, setUsername] = useState('');
    const [email, setEmail] = useState('');
    const [bio, setBio] = useState('');
    const [name, setName] = useState('');
    const [surname, setSurname] = useState('');
    const [university, setUniversity] = useState('');
    const [courseOfStudy, setCourseOfStudy] = useState('');
    const [birthDate, setBirthDate] = useState('');
    const [isEditDialogOpen, setIsEditDialogOpen] = useState(false);
    const [feedbackMessage, setFeedbackMessage] = useState({ type: '', message: '' });
    const [errorMessage, setErrorMessage] = useState('');

    // Form state
    const [formData, setFormData] = useState({
        username: '',
        email: '',
        bio: '',
        name: '',
        surname: '',
        university: '',
        courseOfStudy: '',
        birthDate: '',
    });
    const [password, setPassword] = useState('');

    // Original data state to compare changes
    const [originalData, setOriginalData] = useState({});

    const showFeedback = (type, message) => {
        setFeedbackMessage({ type, message });
        setTimeout(() => setFeedbackMessage({ type: '', message: '' }), 3000);
    };

    const downloadInformations = async () => {
        try {
            const data = Cookies.get('authData');
            const authData = JSON.parse(data);

            const response = await fetch(`${API_SERVER_URL}/api/profile/student`, {
                method: 'GET',
                headers: {
                    'Authorization': `Bearer ${authData.token}`,
                },
            });

            if (response.status === 200) {
                const data = await response.json();
                setUsername(data.username);
                setEmail(data.email);
                setBio(data.bio);
                setName(data.name);
                setSurname(data.surname);
                setUniversity(data.university);
                setCourseOfStudy(data.courseOfStudy);
                setBirthDate(data.birthDate.split('T')[0]);

                const initialFormData = {
                    username: data.username,
                    email: data.email,
                    bio: data.bio,
                    name: data.name,
                    surname: data.surname,
                    university: data.university,
                    courseOfStudy: data.courseOfStudy,
                    birthDate: data.birthDate.split('T')[0],
                };

                setFormData(initialFormData);
                setOriginalData(initialFormData);
            } else {
                console.error('Error checking profile informations:', response.status);
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
                if (key === 'birthDate' && formData[key]) {
                    changedFields[key] = new Date(formData[key]).toISOString();
                } else {
                    changedFields[key] = formData[key];
                }
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

            const response = await fetch(`${API_SERVER_URL}/api/profile/student`, {
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
        <div className="relative">
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

            <div className="bg-white flex flex-col rounded-xl p-6 border items-center">
                <div className="flex flex-col pb-4">
                    <img
                        className="w-24 h-24 border rounded-full"
                        src="https://static.vecteezy.com/system/resources/previews/020/911/733/non_2x/profile-icon-avatar-icon-user-icon-person-icon-free-png.png"
                        alt="Profile"
                    />
                </div>
                <div className="flex flex-col items-center gap-2">
                    <p className="text-lg font-bold">Username: <span className="font-light">{username}</span></p>
                    <p className="text-lg font-bold">Email: <span className="font-light">{email}</span></p>
                    <p className="text-lg font-bold">Name: <span className="font-light">{name}</span></p>
                    <p className="text-lg font-bold">Surname: <span className="font-light">{surname}</span></p>
                    <p className="text-lg font-bold">University: <span className="font-light">{university}</span></p>
                    <p className="text-lg font-bold">Course of study: <span className="font-light">{courseOfStudy}</span></p>
                    <p className="text-lg font-bold">Birthdate: <span className="font-light">{birthDate}</span></p>
                    <p className="text-lg font-bold">Your bio: <span className="font-light">{bio}</span></p>
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
                            <h2 className="text-xl font-bold">Edit Profile</h2>
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
                                <label className="block text-sm font-medium">Name</label>
                                <input
                                    type="text"
                                    name="name"
                                    value={formData.name}
                                    onChange={handleInputChange}
                                    className="w-full border rounded-lg p-2"
                                    placeholder="Name"
                                />
                            </div>
                            <div className="space-y-2">
                                <label className="block text-sm font-medium">Surname</label>
                                <input
                                    type="text"
                                    name="surname"
                                    value={formData.surname}
                                    onChange={handleInputChange}
                                    className="w-full border rounded-lg p-2"
                                    placeholder="Surname"
                                />
                            </div>
                            <div className="space-y-2">
                                <label className="block text-sm font-medium">University</label>
                                <input
                                    type="text"
                                    name="university"
                                    value={formData.university}
                                    onChange={handleInputChange}
                                    className="w-full border rounded-lg p-2"
                                    placeholder="University"
                                />
                            </div>
                            <div className="space-y-2">
                                <label className="block text-sm font-medium">Course of Study</label>
                                <input
                                    type="text"
                                    name="courseOfStudy"
                                    value={formData.courseOfStudy}
                                    onChange={handleInputChange}
                                    className="w-full border rounded-lg p-2"
                                    placeholder="Course of Study"
                                />
                            </div>
                            <div className="space-y-2">
                                <label className="block text-sm font-medium">Birth Date</label>
                                <input
                                    type="date"
                                    name="birthDate"
                                    value={formData.birthDate}
                                    onChange={handleInputChange}
                                    className="w-full border rounded-lg p-2"
                                />
                            </div>
                            <div className="space-y-2">
                                <label className="block text-sm font-medium">Bio</label>
                                <textarea
                                    name="bio"
                                    value={formData.bio}
                                    onChange={handleInputChange}
                                    className="w-full border rounded-lg p-2 h-24 resize-none"
                                    placeholder="Write something about yourself"
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

export default CardProfileStudent;