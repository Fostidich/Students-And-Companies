function RegisterForm() {
    return (
        <div className="bg-white flex flex-col rounded-md p-6 border border-[#d9d9d9]">
            <input type="text" placeholder="Username" className="mb-4 px-4 py-2 border rounded-md"/>
            <input type="email" placeholder="Email" className="mb-4 px-4 py-2 border rounded-md"/>
            <select className="mb-4 px-4 py-2 border rounded-md">
                <option value="student" className="text-black">Student</option>
                <option value="company" className="text-black">Company</option>
            </select>
            <input type="password" placeholder="Password" className="mb-4 px-4 py-2 border rounded-md"/>
            <button
                className="px-24 py-3 text-lg font-light text-white bg-[#2c2c2c] rounded-md hover:bg-[#1e1e1e]"
            >
                Submit
            </button>
        </div>
    );
}

export default RegisterForm;