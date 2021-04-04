import { AppStateProvider } from "../providers/AppStateProvider";
import "../theme/_global.scss";

export default function MyApp({ Component, pageProps }) {
    return (
        <AppStateProvider>
            <Component {...pageProps} />
        </AppStateProvider>
    );
};