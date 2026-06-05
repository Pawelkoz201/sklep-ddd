import OrderHistory from "../components/OrderHistory";
import ProfileC from "../components/ProfileC";
import Profile from "../components/ProfileForm";
import "../styles/ProfilePage.css"; // Import styl�w

export default function ProfilePage() {
    return (
        <div className="profile-page">
            <div className="profile-page-container">
                <Profile />
                <OrderHistory />
            </div>
        </div>
    );
}