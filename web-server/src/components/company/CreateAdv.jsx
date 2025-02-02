import { useState } from 'react';
import Cookies from 'js-cookie';
import {getErrorMessage} from "../../utils/errorUtils.js";
const API_SERVER_URL = window.env?.VITE_API_SERVER_URL || 'http://localhost:5000';

function CreateAdv() {
    const [name, setName] = useState('');
    const [description, setDescription] = useState('');
    const [duration, setDuration] = useState(0);
    const [spots, setSpots] = useState(0);
    const [questionnaire, setQuestionnaire] = useState('');
    const [skills, setSkills] = useState([]);
    const [newSkill, setNewSkill] = useState('');
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);

    const handleAddSkill = (e) => {
        e.preventDefault();
        if (!newSkill.trim()) return;
        setSkills([...skills, newSkill]);
        setNewSkill('');
    };

    const handleDeleteSkill = (skillToDelete) => {
        setSkills(skills.filter(skill => skill !== skillToDelete));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        setError(null);

        try {
            const authData = JSON.parse(Cookies.get('authData'));
            const response = await fetch(`${API_SERVER_URL}/api/recommendation/advertisements`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${authData.token}`,
                },
                body: JSON.stringify({
                    name,
                    description,
                    duration: parseInt(duration, 10),
                    spots: parseInt(spots, 10),
                    questionnaire,
                    skills
                }),
            });

            if (!response.ok) {
                setError(await getErrorMessage(response));
                throw new Error(`HTTP error! Status: ${response.status}`);
            }else{
                setName('');
                setDescription('');
                setDuration(0);
                setSpots(0);
                setQuestionnaire('');
                setSkills([]);
            }
            console.log('Advertisement created successfully');
        } catch (err) {
            console.error('Error creating advertisement:', err);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className={"w-full"}>
            <form onSubmit={handleSubmit} className="flex flex-col gap-4">
                <div>
                    <p className="block font-bold text-2xl p-3">Create a new internship</p>
                    <label className="block font-medium">Name</label>
                    <input
                        type="text"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        className="border rounded p-2 w-full"
                        required
                    />
                </div>
                <div>
                    <label className="block font-medium">Description</label>
                    <input
                        type="text"
                        value={description}
                        onChange={(e) => setDescription(e.target.value)}
                        className="border rounded p-2 w-full"
                        required
                    />
                </div>
                <div>
                    <label className="block font-medium">Duration</label>
                    <input
                        type="number"
                        value={duration}
                        onChange={(e) => setDuration(e.target.value)}
                        className="border rounded p-2 w-full"
                        required
                    />
                </div>
                <div>
                    <label className="block font-medium">Spots</label>
                    <input
                        type="number"
                        value={spots}
                        onChange={(e) => setSpots(e.target.value)}
                        className="border rounded p-2 w-full"
                        required
                    />
                </div>
                <div>
                    <label className="block font-medium">Questionnaire</label>
                    <input
                        type="text"
                        value={questionnaire}
                        onChange={(e) => setQuestionnaire(e.target.value)}
                        className="border rounded p-2 w-full"
                        required
                    />
                </div>
                <div>
                    <label className="block font-medium">Skills</label>
                    <div className="flex gap-2 mb-4">
                        <input
                            type="text"
                            value={newSkill}
                            onChange={(e) => setNewSkill(e.target.value)}
                            className="border rounded p-2 flex-grow"
                            placeholder="Add a skill you are looking for"
                        />
                        <button
                            type="button"
                            onClick={handleAddSkill}
                            className="bg-blue-500 text-white px-4 py-2 rounded"
                        >
                            Add
                        </button>
                    </div>
                    <div className="flex flex-wrap gap-2">
                        {skills.map((skill, index) => (
                            <span key={index} className="bg-gray-200 rounded-full px-3 py-1 flex items-center">
                                {skill}
                                <button
                                    type="button"
                                    onClick={() => handleDeleteSkill(skill)}
                                    className="ml-2 text-red-500"
                                >
                                    x
                                </button>
                            </span>
                        ))}
                    </div>
                </div>
                {error && <div className="text-red-500">{error}</div>}
                <button type="submit" className="bg-green-500 text-white px-4 py-2 rounded" disabled={loading}>
                    {loading ? 'Submitting...' : 'Submit'}
                </button>
            </form>
        </div>
    );
}

export default CreateAdv;
