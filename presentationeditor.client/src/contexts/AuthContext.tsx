import { jwtDecode } from 'jwt-decode';
import React, { createContext, useState, useContext, useEffect } from 'react';

type Payload = { name: string, sub: string }
export interface User {
    id: string;
    name: string;
}
interface AuthContextType {
    user: User | null;
    login: (userName: string) => Promise<void>;
    logout: () => void;
    isAuthenticated: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [user, setUser] = useState<User | null>(null);
    const [isLoading, setIsLoading] = useState(true);

    useEffect(() => {
        // Check if user is logged in on initial load
        (async () => {
            const token = localStorage.getItem('token');
            if (token) {
                var tokenPayload = jwtDecode<Payload>(token);
                var user = {
                    id: tokenPayload.sub,
                    name: tokenPayload.name
                };
                setUser(user);
            }
            setIsLoading(false);
        })()
    }, []);


    const login = async (userName: string) => {
        const response = await fetch(`/api/User/Login/?userName=${userName}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
        });

        if (response.ok) {
            const { token } = await response.json();
            localStorage.setItem('token', token);
            var tokenPayload = jwtDecode<Payload>(token);
            var user = {
                id: tokenPayload.sub,
                name: tokenPayload.name
            };
            setUser(user);
        } else {
            throw new Error(await response.text());
        }
    };

    const logout = () => {
        localStorage.removeItem('token');
        setUser(null);
    };

    const value = {
        user,
        login,
        logout,
        isAuthenticated: !!user,
    };

    if (isLoading) {
        return <div className="App">Loading...</div>;
    }
    return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuth = () => {
    const context = useContext(AuthContext);
    if (context === undefined) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
};