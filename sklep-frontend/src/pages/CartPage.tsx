import TopNavbar from "../components/topnav";
import Footer from "../components/footer";
import CartPage from "../components/cart";


export default function HomePage() {
    return (
        <div>
            <CartPage />
            <Footer />
        </div>
    );
}