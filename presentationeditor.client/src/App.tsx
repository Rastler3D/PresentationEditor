import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import { AuthorizeRoute } from './components/AuthorizeRoute';
import { Login } from './components/Login';
import { AuthProvider } from './contexts/AuthContext';
import { PresentationList } from './components/PresentationList';



function App() {
    return (
        <AuthProvider>
            <Router>
                <Routes>
                    <Route path="/login" element={<Login />} />
                    <Route element={<AuthorizeRoute />}>
                        <Route path="/" element={<PresentationList />} />
                    </Route>
                </Routes>
            </Router>
        </AuthProvider>
    )
}

export default App;