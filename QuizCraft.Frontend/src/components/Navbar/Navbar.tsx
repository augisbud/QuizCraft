import { useState } from "react";
import { Link } from "react-router-dom";
import userIcon from "../../assets/userIcon.svg";
import burgerMenu from "../../assets/burgerMenu.svg";
import styles from "./Navbar.module.scss"; // Import the SCSS module

export const Navbar = () => {
  const [isOpen, setIsOpen] = useState(false);

  const toggleMenu = () => {
    setIsOpen(!isOpen);
  };

  return (
    <nav className={styles.navbar}>
      <div className={styles.navbarContainer}>
        <div className={styles.burger} onClick={toggleMenu}>
          <img src={burgerMenu} alt="Menu" />
        </div>

        <div className={styles.userIcon}>
          <Link to={"/signin"}>
            <img src={userIcon} alt="User Icon" />
          </Link>
        </div>
      </div>

      <div
        className={`${styles.sideMenu} ${isOpen ? styles.sideMenuOpen : ""}`}
      >
        <ul>
          <li>
            <Link className={styles.navbarLink} to={"/"}>
              Home
            </Link>
          </li>
          <li>
            <Link className={styles.navbarLink} to={"/quizzes"}>
              Quizzes
            </Link>
          </li>
        </ul>
      </div>
    </nav>
  );
};
