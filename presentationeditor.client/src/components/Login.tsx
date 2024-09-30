import React, { useState } from 'react';
import { useAuth } from '../contexts/AuthContext';
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { useNavigate } from "react-router-dom";
import { AlertCircle } from 'lucide-react'

export const Login: React.FC = () => {
    const [userName, setUserName] = useState('');
    const [error, setError] = useState('');
    const { login } = useAuth();
    const navigate = useNavigate();

    const handleEnter = async (e: React.FormEvent) => {
        e.preventDefault();
        setError('');
        try {
            await login(userName);
            navigate("/");
        } catch (err) {
            if (err instanceof Error) {
                setError(err.message);
            }
        }
    };

    return (
        <div className="flex flex-col h-screen">
            <div className="flex items-center justify-center h-full">
                <div className="w-full max-w-md space-y-4">
                    <h1 className="text-2xl font-bold text-center">Enter Your Username</h1>
                    <Input
                        type="text"
                        placeholder="Username"
                        value={userName}
                        onChange={(e) => setUserName(e.target.value)}
                    />
                    {error && (
                        <div className="text-red-500 flex items-center">
                            <AlertCircle className="w-4 h-4 mr-2" />
                            {error}
                        </div>
                    )}
                    <Button className="w-full" onClick={handleEnter}>Enter</Button>
                </div>
            </div>
        </div>
    )
};