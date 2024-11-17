import { useEffect, useState } from "react";
import { ReactiveVar } from "./ReactiveVar";

export const useReactiveVar = <T>(reactiveVar: ReactiveVar<T>): T => {
  const [value, setValue] = useState(reactiveVar.get());

  useEffect(() => {
    const unsubscribe = reactiveVar.subscribe(setValue);
    return unsubscribe; // Cleanup on unmount
  }, [reactiveVar]);

  return value;
};
