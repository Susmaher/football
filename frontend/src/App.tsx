import { Route, Routes } from "react-router-dom";
import "./App.css";
import Home from "./Pages/Home";
import Teams from "./Pages/Teams";
import Matches from "./Pages/Matches";
import Admin from "./Pages/Admin";
import Draw from "./Pages/AdminChildPages/Draw";

function App() {
    return (
        <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/teams" element={<Teams />} />
            <Route path="matches" element={<Matches />} />
            <Route path="admin" element={<Admin />}>
                <Route path="draw" element={<Draw />} />
            </Route>
        </Routes>
    );
}

export default App;
