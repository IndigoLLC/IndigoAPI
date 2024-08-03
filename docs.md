## API Documentation

### Setup

1. **Add the API DLL Reference:**
   - Include the API DLL in your project as a reference.
   - Import the necessary namespace in your code:
     ```csharp
     using IndigoAPI;
     ```

2. **Injection:**
   - To inject the exploit, use the following method:
     ```csharp
     ExploitAPI.Attach();
     ```

3. **Script Execution:**
   - To execute a script, use:
     ```csharp
     ExploitAPI.ExecuteScript(textBox1.Text);
     ```
   - For Example If Your Using TextBox You Use This, `textBox1.Text` is a string containing the script you want to run.

4. **Closing Roblox:**
   - To close Roblox, use:
     ```csharp
     ExploitAPI.CloseRoblox();
     ```

5. **Check Injection Status:**
   - To check if the exploit is injected, use:
     ```csharp
     bool isInjected = ExploitAPI.InjectionStatus();
     ```
   - This returns `true` if the exploit is injected and `false` otherwise.

### Support
If you have any questions or need further assistance, please join our [Discord server](https://discord.gg/getindigo).
