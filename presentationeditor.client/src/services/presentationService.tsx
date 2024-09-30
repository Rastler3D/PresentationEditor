
export interface Presentation {
    id: string
    name: string
}

export const createPresentation = async (name: string): Promise<string> => {
    const response = await fetch(`/api/Presentation?name=${name}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token()}`
        },
        body: JSON.stringify({ name }),
    });

    if (response.ok) {
        const { presentation } = await response.json();
        return presentation.id;
    } else {
        throw new Error(await response.text());
    }
};

export const getPresentations = async (): Promise<Presentation[]> => {
    const response = await fetch('/api/Presentation', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token()}`
        },
    });

    if (response.ok) {
        const presentations = await response.json();
        return presentations;
    } else {
        throw new Error(await response.text());
    }
};


export const token = () => localStorage.getItem('token') ?? "";