import { PlusCircle, Grid, List } from 'lucide-react'

import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { ScrollArea } from "@/components/ui/scroll-area"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import { Label } from "@/components/ui/label"
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
    DialogTrigger,
} from "@/components/ui/dialog"
import { useEffect, useState } from 'react'
import { useNavigate } from "react-router-dom";
import { createPresentation, getPresentations, Presentation } from '../services/presentationService'


export const PresentationList: React.FC = () => {
    const navigate = useNavigate();
    const [presentations, setPresentations] = useState([] as Presentation[])
    const [newPresentationName, setNewPresentationName] = useState('')

    const handleJoinPresentation = (presentationId: string) => {
        navigate(`/presentation/${presentationId}`);
    }
    const handleCreatePresentation = async () => {
        const presentationId = await createPresentation(newPresentationName);
        handleJoinPresentation(presentationId);
    }

    useEffect(() => {
        (async () => {
            const presentations = await getPresentations();
            setPresentations(presentations);
        })();
    }, []);

    return (
        <div className="flex flex-col h-screen">
            <div className="flex flex-col h-full p-4 space-y-4">
                <div className="flex justify-between items-center">
                    <h1 className="text-2xl font-bold">Presentations</h1>
                    <Dialog>
                        <DialogTrigger asChild>
                            <Button>
                                <PlusCircle className="mr-2 h-4 w-4" /> Create Presentation
                            </Button>
                        </DialogTrigger>
                        <DialogContent className="sm:max-w-[425px]">
                            <DialogHeader>
                                <DialogTitle>Create New Presentation</DialogTitle>
                                <DialogDescription>
                                    Enter a name for your new presentation.
                                </DialogDescription>
                            </DialogHeader>
                            <div className="grid gap-4 py-4">
                                <div className="grid grid-cols-4 items-center gap-4">
                                    <Label htmlFor="name" className="text-right">
                                        Name
                                    </Label>
                                    <Input
                                        id="name"
                                        value={newPresentationName}
                                        onChange={(e) => setNewPresentationName(e.target.value)}
                                        className="col-span-3"
                                    />
                                </div>
                            </div>
                            <DialogFooter>
                                <Button type="submit" onClick={handleCreatePresentation}>Create</Button>
                            </DialogFooter>
                        </DialogContent>
                    </Dialog>
                </div>
                <div className="flex space-x-4">
                    <Input type="text" placeholder="Search presentations..." className="flex-grow" />
                    <Tabs defaultValue="table">
                        <TabsList>
                            <TabsTrigger value="table"><List className="h-4 w-4" /></TabsTrigger>
                            <TabsTrigger value="tiles"><Grid className="h-4 w-4" /></TabsTrigger>
                        </TabsList>
                    </Tabs>
                </div>
                <ScrollArea className="flex-grow">
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                        presentations? {presentations?.map((presentation) => (
                            <div key={presentation.id} className="border p-4 rounded-lg">
                                <h2 className="text-lg font-semibold">{presentation.name}</h2>
                                <Button onClick={() => handleJoinPresentation(presentation.id)}>Join</Button>
                            </div>
                        ))}
                        : <h2 className="border p-4 rounded-lg">No presentations</h2>
                    </div>
                </ScrollArea>
            </div>
        </div>
    )
}