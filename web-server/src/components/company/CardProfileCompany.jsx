import { useState, useEffect } from 'react';
import Cookies from 'js-cookie';
const API_SERVER_URL = window.env?.VITE_API_SERVER_URL || 'http://localhost:5000';

function CardProfileCompany() {
    const [isEditOpen, setIsEditOpen] = useState(false);
    const [profileData, setProfileData] = useState({
        username: '',
        email: '',
        password: '',
        bio: '',
        headquarter: '',
        fiscalCode: '',
        vatNumber: ''
    });
    const [editData, setEditData] = useState({
        username: '',
        email: '',
        password: '',
        bio: '',
        headquarter: '',
        fiscalCode: '',
        vatNumber: ''
    });

    useEffect(() => {
        downloadInformations();
    }, []);

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
                setEditData(data);
            }
        } catch (error) {
            console.error('Error checking profile informations:', error);
        }
    };

    const handleSubmit = async () => {
        try {
            const authData = JSON.parse(Cookies.get('authData'));
            const response = await fetch(`${API_SERVER_URL}/api/profile/company`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${authData.token}`,
                },
                body: JSON.stringify(editData),
            });
            console.log(JSON.stringify(editData));

            if (response.status === 200) {
                setProfileData(editData);
                setIsEditOpen(false);
            }
        } catch (error) {
            console.error('Error updating profile:', error);
        }
    };

    return (
        <>
            <div className="bg-white flex flex-col rounded-xl p-6 border items-center w-full h-full">
                <div className="flex flex-col pb-4">
                    <img className="w-24 h-24 border rounded-full"
                         src="https://static.vecteezy.com/system/resources/previews/020/911/733/non_2x/profile-icon-avatar-icon-user-icon-person-icon-free-png.png"
                         alt="Profile"/>
                </div>
                <div className="flex flex-col items-center gap">
                    <p className="text-lg font-bold">Username: <span className="font-light">{profileData.username}</span></p>
                    <p className="text-lg font-bold">Email: <span className="font-light">{profileData.email}</span></p>
                    <p className="text-lg font-bold">Head quarter: <span className="font-light">{profileData.headquarter}</span></p>
                    <p className="text-lg font-bold">Fiscal code: <span className="font-light">{profileData.fiscalCode}</span></p>
                    <p className="text-lg font-bold">Vat number: <span className="font-light">{profileData.vatNumber}</span></p>
                    <p className="text-lg font-bold">Your vision: <span className="font-light">{profileData.bio}</span></p>
                </div>
                <button className="rounded-xl underline text-white" onClick={() => setIsEditOpen(true)}>Edit ma non funziona bene lato server</button>
            </div>

            {isEditOpen && (
                <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
                    <div className="bg-white p-6 rounded-lg w-[425px] max-h-[90vh] overflow-y-auto">
                        <h2 className="text-xl font-bold mb-4">Edit Profile</h2>
                        <div className="flex flex-col gap-4">
                            <input
                                type="text"
                                value={editData.username}
                                onChange={e => setEditData({...editData, username: e.target.value})}
                                className="border rounded p-2"
                                placeholder="Username"
                            />
                            <input
                                type="email"
                                value={editData.email}
                                onChange={e => setEditData({...editData, email: e.target.value})}
                                className="border rounded p-2"
                                placeholder="Email"
                            />
                            <input
                                type="text"
                                value={editData.password}
                                onChange={e => setEditData({...editData, password: e.target.value})}
                                className="border rounded p-2"
                                placeholder="New password"
                            />
                            <input
                                type="text"
                                value={editData.headquarter}
                                onChange={e => setEditData({...editData, headquarter: e.target.value})}
                                className="border rounded p-2"
                                placeholder="Headquarter"
                            />
                            <input
                                type="text"
                                value={editData.fiscalCode}
                                onChange={e => setEditData({...editData, fiscalCode: e.target.value})}
                                className="border rounded p-2"
                                placeholder="Fiscal Code"
                            />
                            <input
                                type="text"
                                value={editData.vatNumber}
                                onChange={e => setEditData({...editData, vatNumber: e.target.value})}
                                className="border rounded p-2"
                                placeholder="VAT Number"
                            />
                            <textarea
                                value={editData.bio}
                                onChange={e => setEditData({...editData, bio: e.target.value})}
                                className="border rounded p-2"
                                placeholder="Bio"
                                rows="3"
                            />
                            <div className="flex justify-end gap-2 mt-4">
                                <button
                                    onClick={() => setIsEditOpen(false)}
                                    className="px-4 py-2 border rounded hover:bg-gray-100"
                                >
                                    Cancel
                                </button>
                                <button
                                    onClick={handleSubmit}
                                    className="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600"
                                >
                                    Save Changes
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </>
    );
}

export default CardProfileCompany;
