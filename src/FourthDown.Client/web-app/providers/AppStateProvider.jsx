import { createContext, useState } from "react";

export const AppStateContext = createContext({});

export const AppStateProvider = ({ children }) => {
    const [season, setSeason] = useState(2020);

    return (
        <AppStateContext.Provider value={{
            season,
            setSeason
        }}>
            {children}
        </AppStateContext.Provider>
    );
};