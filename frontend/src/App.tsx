import { Route, Routes } from "react-router-dom";
import "./App.css";
import Home from "./Pages/Home";
import Teams from "./Pages/Teams";
import Matches from "./Pages/Matches";
import Admin from "./Pages/Admin";
import Draw from "./Pages/AdminChildPages/Draw";
import AdminTeams from "./Pages/AdminChildPages/AdminTeams";
import AdminDivisions from "./Pages/AdminChildPages/AdminDivisions";
import AdminFields from "./Pages/AdminChildPages/AdminFields";

function App() {
    return (
        <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/teams" element={<Teams />} />
            <Route path="/matches" element={<Matches />} />
            <Route path="/admin" element={<Admin />}>
                <Route path="draw" element={<Draw />} />
                <Route path="teams" element={<AdminTeams />} />
                <Route path="divisions" element={<AdminDivisions />} />
                <Route path="fields" element={<AdminFields />} />
            </Route>
        </Routes>
    );
}

export default App;
