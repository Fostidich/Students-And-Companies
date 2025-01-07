import Card from "../components/Card.jsx";

function Home({ jobs }) {
    return (
        <div className="pt-8 px-4 flex justify-center ">
            <div className="grid grid-cols-3 gap-8 max-w-6xl">
                {jobs.map((job) =>
                    <Card
                        key={job.id}
                        title={job.title}
                        imgURL={job.imgURL}
                        jobPosition={job.jobPosition}
                    />)}
            </div>
        </div>
    );
}

export default Home;