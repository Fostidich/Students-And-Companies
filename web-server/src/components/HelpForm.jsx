import  { useState } from "react";

const Form = () => {
    const [text, setText] = useState("");

    const handleCancel = () => {
        setText(""); // Resetta il testo
    };

    const handleSubmit = () => {
        console.log("Submitted:", text); // Azione di submit (può essere modificata)
    };

    return (
        <div className="max-w-md mx-auto mt-10 p-6 border-slate-300 bg-white shadow-md rounded-lg">
            <div className="flex-col justify-start items-start gap-2 flex">
                <h1 className="text-xl font-semibold mb-4 text-gray-800">Are you sure absolutely sure?</h1>
                <p className="text-slate-500 text-sm font-normal leading-tight">This action cannot be undone. This will open a complaint with your internship company.</p>
            </div>
            <textarea
                className="w-full p-3 border border-gray-300 rounded-md shadow-sm focus:ring-2 focus:ring-blue-500 focus:outline-none resize-none"
                placeholder="Start typing..."
                rows={5}
                value={text}
                onChange={(e) => setText(e.target.value)}
                style={{
                    minHeight: "120px",
                    maxHeight: "300px",
                    overflowY: "auto",
                }}
            ></textarea>
            <div className="flex justify-end gap-4 mt-4">
                <button
                    type="button"
                    className="px-4 py-2 bg-gray-500 text-white rounded-md hover:bg-gray-600 focus:ring-2 focus:ring-gray-300"
                    onClick={handleCancel}
                >
                    Cancel
                </button>
                <button
                    type="button"
                    className="px-4 py-2 bg-red-500 text-white rounded-md hover:bg-red-600 focus:ring-2 focus:ring-red-300"
                    onClick={handleSubmit}
                >
                    Continue
                </button>
            </div>
        </div>
    );
};

export default Form;
