import { Button } from "@/components/ui/button"
import { Settings, Download, Users } from "lucide-react";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { User } from "../contexts/AuthContext";
import { Presentation, token } from "../services/presentationService";
import signalR from "@microsoft/signalr";

export const PresentationList: React.FC = () => {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/hub/presentation", { accessTokenFactory: token  })
        .withAutomaticReconnect()
        .build();
    const navigate = useNavigate();
    const [snapshot, setSnapshot] = useState(null);
    const [presentations, setPresentations] = useState([] as Presentation[])
    const [newPresentationName, setNewPresentationName] = useState('')

    connection.on("PresentationSnapshot", (snapshot) => {
        setSnapshot(snapshot)
    });

    return (
        <div className="flex flex-col h-screen">

            <div className="flex flex-col h-full">
                <div className="flex items-center justify-between p-2 border-b">
                    <h1 className="text-xl font-bold">{currentPresentation.name}</h1>
                    <div className="flex items-center space-x-2">
                        <Button variant="outline" size="sm">
                            <Settings className="mr-2 h-4 w-4" /> Tools
                        </Button>
                        <Button variant="outline" size="sm">
                            <Download className="mr-2 h-4 w-4" /> Export to PDF
                        </Button>
                    </div>
                </div>
                <div className="flex flex-1 overflow-hidden">
                    <div className="w-64 border-r p-2 overflow-y-auto">
                        <Button className="w-full mb-2" onClick={handleAddSlide}>Add Slide</Button>
                        {currentPresentation.slides.map((slide, index) => (
                            <div key={slide.id} className="border p-2 mb-2 cursor-pointer">
                                Slide {index + 1}
                            </div>
                        ))}
                    </div>
                    <div className="flex-1 p-4 overflow-auto">
                        {/* Main slide editing area */}
                        <div className="border h-full flex items-center justify-center text-2xl text-gray-400">
                            Slide Content Area
                        </div>
                    </div>
                    <div className="flex-1 overflow-hidden">
                        {currentSlide && (
                            <Tldraw
                                showMenu={false}
                                showPages={false}
                                showStyles={false}
                                showZoom={false}
                                showTools={true}
                                showUI={true}
                                readOnly={false}
                                onMount={(app) => {
                                    app.loadSnapshot(currentSlide.content)
                                }}
                                onChange={handleTldrawChange}
                                {...fileSystemEvents}
                            />
                        )}
                    </div>
                    <div className="w-64 border-l p-2 overflow-y-auto">
                        <h2 className="font-semibold mb-2 flex items-center">
                            <Users className="mr-2 h-4 w-4" /> Connected Users
                        </h2>
                        {currentPresentation.users.map((user) => (
                            <div key={user.id} className="flex items-center justify-between mb-2">
                                <span>{user.name}</span>
                                <select
                                    value={user.role}
                                    onChange={(e) => handleChangeUserRole(user.id, e.target.value as User['role'])}
                                    className="text-sm border rounded"
                                >
                                    <option value="Viewer">Viewer</option>
                                    <option value="Editor">Editor</option>
                                </select>
                            </div>
                        ))}
                    </div>
                </div>
            </div>
        </div>
    )
}