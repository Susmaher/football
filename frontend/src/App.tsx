import { Route, Routes } from "react-router-dom";
import "./App.css";
import Home from "./Pages/Home";
import Teams from "./Pages/Teams";
import Matches from "./Pages/Matches";

/*
    interface DivisionData {
    id: string;
    name: string;
    }
    
    
    const [divisions, setDivisions] = useState<DivisionData[]>([]);

    const fetchData = async () => {
        try {
            const response = await api.get("Divisions");
            console.log(response.data);
            setDivisions(response.data);
        } catch (err) {
            console.log("Something went wrong fetching notes:", err);
        }
    };

    useEffect(() => {
        const fetching = async () => {
            await fetchData();
        };
        fetching();
    }, []);
    
    


    <h1>Hi</h1>
            {divisions ? (
                divisions.map((division: DivisionData) => (
                    <div key={division.id}>
                        <p>Division: {division.name}</p>
                    </div>
                ))
            ) : (
                <p>Loading...</p>
            )}
    */

function App() {
    return (
        <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/teams" element={<Teams />} />
            <Route path="matches" element={<Matches />} />
        </Routes>
    );
}

export default App;
