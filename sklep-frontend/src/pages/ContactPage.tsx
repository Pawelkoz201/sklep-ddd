import TopNavbar from "../components/topnav";
import Footer from "../components/footer";
import ContactPage from "../components/ContactForm";
import { MapContainer } from "react-leaflet";


export default function HomePage() {
    return (
        <div>
            <ContactPage />            
            <Footer />
        </div>
    );
}
