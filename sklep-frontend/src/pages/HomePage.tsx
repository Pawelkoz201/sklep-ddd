import TopNavbar from "../components/topnav";
import Footer from "../components/footer";
import Home from "../components/home";
import { useUser } from "../services/userContext"

export default function HomePage() {
  const { user } = useUser();
  console.log(user.name);
  return (
      <div>         
          <Home />
          <Footer />
    </div>
  );
}

