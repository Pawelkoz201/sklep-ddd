import React, { createContext, useContext, useState, useEffect, ReactNode } from "react";

interface CartItem {
  id: number;
  name: string;
  quantity?: number;
}

interface User {
  id: number,
  name: string;
  isLoggedIn: boolean;
  cartItems: CartItem[];
  token?: string; // Optional token
}

interface UserContextValue {
  user: User;
  setUser: React.Dispatch<React.SetStateAction<User>>;
}

const UserContext = createContext<UserContextValue | undefined>(undefined);

export function useUser(): UserContextValue {
  const context = useContext(UserContext);
  if (!context) {
    throw new Error("useUser must be used within a UserProvider");
  }
  return context;
}

export default function UserProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User>(() => {
    const storedUser = localStorage.getItem("user");
    return storedUser
      ? JSON.parse(storedUser)
      : { isLoggedIn: false, cartItems: [] };
  });

  useEffect(() => {
    localStorage.setItem("user", JSON.stringify(user));
  }, [user]);

  return (
    <UserContext.Provider value={{ user, setUser }}>
      {children}
    </UserContext.Provider>
  );
}
